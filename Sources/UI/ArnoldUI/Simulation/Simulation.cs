﻿using System;
using System.Threading.Tasks;
using GoodAI.Arnold.Network;
using GoodAI.Arnold.Simulation;
using GoodAI.Arnold.Extensions;
using GoodAI.Arnold.Project;

namespace GoodAI.Arnold.Simulation
{
    public interface ISimulation
    {
        event EventHandler<StateUpdatedEventArgs> StateUpdated;
        event EventHandler<StateChangeFailedEventArgs> StateChangeFailed;

        /// <summary>
        /// Loads an agent into the handler, creates a new simulation.
        /// This moves the simulation from Empty state to Paused.
        /// </summary>
        void LoadBlueprint(AgentBlueprint agentBlueprint);

        /// <summary>
        /// Runs the given number of steps.
        /// This moves the simulation from Paused to Running. When the requested number
        /// of steps are performed, moves to state Paused.
        /// </summary>
        /// <param name="stepsToRun">The number of steps to run. 0 is infinity.</param>
        void Run(uint stepsToRun = 0);

        /// <summary>
        /// Pauses the running simulation. If the simulation is not running, this does nothing.
        /// This moves the simulation from Running state to Paused.
        /// </summary>
        void Pause();

        /// <summary>
        /// This moves the simulation from Running or Paused state to Empty.
        /// </summary>
        void Clear();

        /// <summary>
        /// Requests a state refresh.
        /// </summary>
        void RefreshState();

        SimulationState State { get; }

        ISimulationModel Model { get; }
    }

    public enum SimulationState
    {
        Null,  // The simulation doesn't exist. Don't set this as Simulation.State!
        Empty,  // The core is ready, but no blueprint has been loaded.
        Paused,
        Running,
        ShuttingDown,
        Invalid  // Invalid state - something really wrong happened in the core.
    }

    public class WrongHandlerStateException : Exception
    {
        public WrongHandlerStateException(string message) : base(message) { }

        public WrongHandlerStateException(string methodName, SimulationState state)
            : base($"Cannot run {methodName}, simulation is in {state} state.")
        { }
    }

    public class SimulationProxy : ISimulation
    {
        public ISimulationModel Model { get; private set; }

        public event EventHandler<StateUpdatedEventArgs> StateUpdated;
        public event EventHandler<StateChangeFailedEventArgs> StateChangeFailed;

        public SimulationState State
        {
            get { return m_state; }
            private set
            {
                if (value == SimulationState.Null)
                    throw new InvalidOperationException("The simulation state cannot be Null.");

                SimulationState oldState = m_state;
                m_state = value;
                StateUpdated?.Invoke(this, new StateUpdatedEventArgs(oldState, m_state));
            }
        }

        private readonly ICoreLink m_coreLink;
        private readonly ICoreController m_controller;
        private SimulationState m_state;

        public SimulationProxy(ICoreLink coreLink, ICoreController controller)
        {
            m_coreLink = coreLink;
            m_controller = controller;
            State = SimulationState.Empty;

            Model = new SimulationModel();
        }

        public void LoadBlueprint(AgentBlueprint agentBlueprint)
        {
            if (State != SimulationState.Empty)
                throw new WrongHandlerStateException("LoadAgent", State);

            // TODO(HonzaS): Add the blueprint data.
            var conversation = new CommandConversation(CommandType.Load);

            SendCommand(conversation);
        }

        public void Clear()
        {
            var conversation = new CommandConversation(CommandType.Clear);

            SendCommand(conversation);
        }

        public void Run(uint stepsToRun = 0)
        {
            if (State != SimulationState.Paused && State != SimulationState.Running)
                throw new WrongHandlerStateException("Run", State);

            RunSimulation(stepsToRun);
        }

        public void Pause()
        {
            if (State != SimulationState.Paused && State != SimulationState.Running)
                throw new WrongHandlerStateException("Pause", State);

            if (State == SimulationState.Paused)
                return;

            var conversation = new CommandConversation(CommandType.Pause);

            SendCommand(conversation);

            // TODO(HonzaS): Signal the Core.
            // TODO(HonzaS): Logging!
            //if (!signalled)
            //    deal with it
        }

        private void RunSimulation(uint stepsToRun)
        {
            if (State == SimulationState.Running)
                return;

            var conversation = new CommandConversation(CommandType.Run, stepsToRun);

            SendCommand(conversation);
        }

        private void SendCommand(CommandConversation conversation)
        {
            m_controller.Command(conversation, HandleStateResponse, HandleTimeoutCancellation);
        }

        private TimeoutAction HandleTimeoutCancellation()
        {
            throw new NotImplementedException();
        }

        private void HandleStateResponse(Response<StateResponse> response)
        {
            if (response.Error != null)
            {
                HandleError(response.Error);
            }
            else
            {
                State = ReadState(response.Data);
            }
        }

        public static SimulationState ReadState(StateResponse stateData)
        {
            switch (stateData.State)
            {
                case StateType.Empty:
                    return SimulationState.Empty;
                case StateType.Running:
                    return SimulationState.Running;
                case StateType.Paused:
                    return SimulationState.Paused;
                case StateType.ShuttingDown:
                    return SimulationState.ShuttingDown;
                case StateType.Invalid:
                    return SimulationState.Invalid;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleError(ErrorResponse error)
        {
            StateChangeFailed?.Invoke(this, new StateChangeFailedEventArgs(error));
        }

        public void RefreshState()
        {
            var conversation = new GetStateConversation();

            m_coreLink.Request(conversation).ContinueWith(task =>
            {
                TimeoutResult<Response<StateResponse>> timeoutResult = task.Result;
                if (!timeoutResult.TimedOut && timeoutResult.Result.Error != null)
                    State = ReadState(timeoutResult.Result.Data);
            });
        }
    }
}
