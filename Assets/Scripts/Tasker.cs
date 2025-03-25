using TMPro;
using UnityEngine;

public class GoalSeeker : MonoBehaviour
{
    Goal[] _goals;
    public TMP_Text _text;
    public TMP_Text actionText;
    Action[] _actions;
    Action mChangeOverTime;
    const float TICK_LENGTH = 5.0f;

    void Start()
    {
        // connect animations for PROD
        _goals = new Goal[3];
        _goals[0] = new Goal("Eat", 4);
        _goals[1] = new Goal("Slide", 3);
        _goals[2] = new Goal("Find Friends", 3);

        _actions = new Action[6];
        _actions[0] = new Action("eat squid NOM");
        //instantiate food
        //anim attack
        _actions[0].targetGoals.Add(new Goal("Eat", -4f));
        _actions[0].targetGoals.Add(new Goal("Slide", +2f));
        _actions[0].targetGoals.Add(new Goal("Find Friends", +1f));

        _actions[1] = new Action("kill a krill for fun");
        //intsantiate food
        //anim jump
        _actions[1].targetGoals.Add(new Goal("Eat", -2f));
        _actions[1].targetGoals.Add(new Goal("Slide", -1f));
        _actions[1].targetGoals.Add(new Goal("Find Friends", +1f));

        _actions[2] = new Action("SLIDE TIME");
        //anim slide long
        _actions[2].targetGoals.Add(new Goal("Eat", +2f));
        _actions[2].targetGoals.Add(new Goal("Slide", -4f));
        _actions[2].targetGoals.Add(new Goal("Find Friends", +1f));

        _actions[3] = new Action("slide for a bit");
        //anim slide short
        _actions[3].targetGoals.Add(new Goal("Eat", +1f));
        _actions[3].targetGoals.Add(new Goal("Slide", -2f));
        _actions[3].targetGoals.Add(new Goal("Find Friends", +1f));

        _actions[4] = new Action("eat fish NOM");
        //instantiate food
        //anim attack
        _actions[4].targetGoals.Add(new Goal("Eat", -2f));
        _actions[4].targetGoals.Add(new Goal("Slide", +2f));
        _actions[4].targetGoals.Add(new Goal("Find Friends", +1f));

        _actions[5] = new Action("go find my friends");
        //anim walk
        _actions[5].targetGoals.Add(new Goal("Eat", 0f));
        _actions[5].targetGoals.Add(new Goal("Slide", +1f));
        _actions[5].targetGoals.Add(new Goal("Find Friends", -4f));

        // goals change as time passes
        mChangeOverTime = new Action("tick");
        mChangeOverTime.targetGoals.Add(new Goal("Eat", +3f));
        mChangeOverTime.targetGoals.Add(new Goal("Slide", +3f));
        mChangeOverTime.targetGoals.Add(new Goal("Find Friends", +2f));

        Debug.Log("Starting clock.");
        InvokeRepeating("Tick", 0f, TICK_LENGTH);

        Debug.Log("Hit E to act.");
    }

    void Tick()
    {
        foreach (Goal goal in _goals)
        {
            goal.value += mChangeOverTime.GetGoalChange(goal);
            goal.value = Mathf.Max(goal.value, 0);
        }
        PrintGoals();
    }

    void PrintGoals()
    {
        string goalString = "";
        foreach (Goal goal in _goals)
        {
            goalString += goal.name + ": " + goal.value + "; ";
        }
        goalString += "Discontentment: " + CurrentDiscontentment();
        _text.text = goalString;
        Debug.Log(goalString);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Action bestTask = ChooseAction(_actions, _goals);
            actionText.text = "methinks I will " + bestTask.name;
            Debug.Log("methinks I will " + bestTask.name);
            foreach (Goal goal in _goals)
            {
                goal.value += bestTask.GetGoalChange(goal);
                goal.value = Mathf.Max(goal.value, 0);
            }
            PrintGoals();
        }
    }

    Action ChooseAction(Action[] actions, Goal[] goals)
    {
        //action to the lowest discontentment
        Action bestAction = null;
        float bestValue = float.PositiveInfinity;

        foreach (Action action in actions)
        {
            float thisValue = Discontentment(action, goals);
            if (thisValue < bestValue)
            {
                bestValue = thisValue;
                bestAction = action;
            }
        }
        return bestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontentment = 0f;
        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue = Mathf.Max(newValue, 0);
            discontentment += goal.GetDiscontentment(newValue);
        }
        return discontentment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f;
        foreach (Goal goal in _goals)
        {
            total += (goal.value * goal.value);
        }
        return total;
    }
}