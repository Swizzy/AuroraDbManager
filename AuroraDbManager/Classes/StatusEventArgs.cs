namespace AuroraDbManager.Classes {
    using System;

    public class StatusEventArgs: EventArgs {
        public readonly string Status;

        public StatusEventArgs(string status) { Status = status; }
    }
}