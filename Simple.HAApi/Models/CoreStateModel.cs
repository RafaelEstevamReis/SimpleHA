namespace Simple.HAApi.Models
{
    public class CoreStateModel
    {
        public string state { get; set; }
        public Recorder_State recorder_state { get; set; }

        public class Recorder_State
        {
            public bool migration_in_progress { get; set; }
            public bool migration_is_live { get; set; }
        }
    }

}
