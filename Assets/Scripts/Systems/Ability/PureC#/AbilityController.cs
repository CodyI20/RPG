using AbilityUtilities;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AbilityController
{
    readonly AbilityModel model;
    readonly AbilityView view;
    readonly Queue<AbilityCommand> abilityQueue = new();

    // Timer for the current action (cast time/animation lock)
    readonly CountdownTimer castTimer = new CountdownTimer(0);

    // Timer for the global cooldown
    readonly CountdownTimer gcdTimer = new CountdownTimer(0);
    readonly float gcdDuration;

    AbilityController(AbilityView view, AbilityModel model, float gcdDuration)
    {
        this.view = view;
        this.model = model;
        this.gcdDuration = gcdDuration;

        ConnectModel();
        ConnectView();
    }

    void ConnectModel()
    {
        model.abilities.AnyValueChanged += UpdateButtons;
    }

    void ConnectView()
    {
        for (int i = 0; i < view.buttons.Length; i++)
        {
            view.buttons[i].RegisterListener(OnAbilityButtonPressed);
        }
        view.UpdateButtonDetails(model.abilities);
    }

    public void Update(float deltaTime)
    {
        castTimer.Tick(deltaTime);
        gcdTimer.Tick(deltaTime);

        // Update individual cooldowns
        foreach (var ability in model.abilities)
        {
            ability.Tick(deltaTime);
        }

        view.UpdateRadial(model.abilities);

        // Process queue if not currently casting
        if (!castTimer.IsRunning && abilityQueue.Count > 0)
        {
            // Peek at the next command
            AbilityCommand cmd = abilityQueue.Peek();

            // Find the corresponding ability to check its state
            // Note: AbilityCommand holds AbilityData, but we need the runtime Ability object to check cooldown.
            // Since we don't have a direct link from Command -> Ability Runtime,
            // we might need to look it up or pass the runtime Ability to the command.
            // However, for now, we can iterate to find it or refactor AbilityCommand.
            // A simpler way: Store the Ability instance in the Command?
            // Or since we are in the Controller, we can find it.

            Ability abilityToCast = FindAbilityForCommand(cmd);

            if (abilityToCast != null)
            {
                // Check if Ability is ready AND GCD is ready
                if (abilityToCast.IsReady && !gcdTimer.IsRunning)
                {
                    abilityQueue.Dequeue(); // Remove from queue
                    cmd.Execute();

                    // Start Cooldowns
                    abilityToCast.StartCooldown();

                    // Start Cast Timer (Duration of the action)
                    castTimer.Reset(cmd.duration);
                    castTimer.Start();

                    // Start GCD
                    gcdTimer.Reset(gcdDuration);
                    gcdTimer.Start();
                }
                // If not ready, we wait. It stays in the queue.
                // Note: If the ability is ON cooldown, it will block the queue.
                // This is consistent with "queueing up" logic.
            }
            else
            {
                // If we can't find the ability (shouldn't happen), drop it.
                abilityQueue.Dequeue();
            }
        }
    }

    private Ability FindAbilityForCommand(AbilityCommand cmd)
    {
        foreach (var ability in model.abilities)
        {
            if (ability.data == cmd.data)
            {
                return ability;
            }
        }
        return null;
    }

    void UpdateButtons(IList<Ability> updatedAbilities) => view.UpdateButtonDetails(updatedAbilities);

    void OnAbilityButtonPressed(int index)
    {
        if (index < 0 || index >= model.abilities.Count) return;

        Ability ability = model.abilities[index];

        // If the ability is currently on cooldown, do not queue it.
        if (!ability.IsReady)
        {
            // Optional: User feedback for "On Cooldown"
            return;
        }

        // If we are here, the ability is ready (off internal cooldown).
        // It might still be blocked by GCD or another cast, so we queue it.
        abilityQueue.Enqueue(ability.CreateCommand());

        EventSystem.current.SetSelectedGameObject(null);
    }

    public class Builder
    {
        readonly AbilityModel model = new AbilityModel();
        float gcdDuration = 1.0f; // Default GCD

        public Builder WithAbilities(AbilityData[] datas)
        {
            foreach (var data in datas)
            {
                model.Add(new Ability(data));
            }
            return this;
        }

        public Builder WithGlobalCooldown(float duration)
        {
            gcdDuration = duration;
            return this;
        }

        public AbilityController Build(AbilityView view)
        {
            Preconditions.CheckNotNull(view);
            return new AbilityController(view, model, gcdDuration);
        }
    }
}
