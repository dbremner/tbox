using System;
using System.Windows.Forms;
using System.Threading;
using Common;
using Common.MT;
using WFControls.OS;

namespace WFControls.Forms
{
	public partial class Progress : Form
	{
		private class Updater : IUpdater
		{
			private readonly Progress m_owner;
			public Updater(Progress owner)
			{
				m_owner = owner;
			}

			public virtual void Update(float value)
			{
				Mt.Do(m_owner.progressBar, () =>
				{
					m_owner.progressBar.Value = (int)
						(m_owner.progressBar.Minimum + m_owner.progressBar.Maximum * value);
				});
			}

			public virtual void Update(string caption, float value)
			{
				Mt.SetText(m_owner.label, caption);
				Update(value);
			}

			public bool UserPressClose { get { return m_owner.UserPressClose; } }
			public void Update(Func<int, string> caption, int current, int total)
			{
				throw new NotImplementedException();
			}
		}

		private class SingleThreadedUpdater : Updater
		{
			public SingleThreadedUpdater(Progress owner) : base(owner)
			{
			}

			public override void Update(string caption, float value)
			{
				base.Update(caption, value);
				Application.DoEvents();
			}
		}

		public Progress()
		{
			InitializeComponent();
		}

		private void Work(object obj)
		{
			var updater = obj as IUpdater;
			UserPressClose = false;
			if (m_func(updater))
			{
				Mt.Do(this, () => { m_canClose = true; });
				Mt.Do(this, Hide);
			}
		}

		private Func<IUpdater, bool> m_func;
		private Thread m_thread;
		private bool m_canClose;
		private bool UserPressClose { get; set; }
		public DialogResult ShowDialog(Func<IUpdater, bool> func, string caption,
			bool cancelEnabled = true, bool multithreaded = true)
		{
			btnCancel.Enabled = cancelEnabled;
			Text = caption;
			m_canClose = false;
			m_func = func;
			if(multithreaded)
			{
				m_thread = new Thread(Work);
				m_thread.Start(new Updater(this));
				return base.ShowDialog();
			}
			else
			{
				Show();
				Work(new SingleThreadedUpdater(this));
				return DialogResult.OK;
			}
		}

		private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !m_canClose;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			UserPressClose = true;
		}
	}
}
