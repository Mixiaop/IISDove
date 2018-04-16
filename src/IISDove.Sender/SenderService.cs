using System.ServiceProcess;
using System.Timers;

namespace IISDove.Sender
{
    partial class SenderService : ServiceBase
    {
        DoveSettings Settings => Current.GetSettings();
        public SenderService()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = Settings.SendInterval;
            timer.Enabled = true;
        }

        protected override void OnStart(string[] args)
        {
            Current.Log(string.Format("Dove sender【{0}_{1}】已启动", Settings.Code, Settings.Name));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Current.SenderCommand.CheckAndSend();
        }

        protected override void OnStop()
        {
            Current.Log(string.Format("Dove sender【{0}】已关闭", Settings.Code, Settings.Name));
        }
    }
}
