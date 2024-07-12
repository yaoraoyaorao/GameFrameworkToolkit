using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class BaseComponent : MonoBehaviour
    {
        [SerializeField]
        private bool runInBackgroun;

        [SerializeField]
        private bool neverSleep;        
        
        [SerializeField]
        private int frameRote;

        /// <summary>
        /// ��ȡ�������Ƿ������̨����
        /// </summary>
        public bool RunInBackgroun
        {
            get
            {
                return runInBackgroun;
            }
            set
            {
                Application.runInBackground = runInBackgroun = value;
            }
        }

        /// <summary>
        /// ��ȡ�������Ƿ���������
        /// </summary>
        public bool NeverSleep
        {
            get
            {
                return neverSleep;
            }
            set
            {
                neverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        /// <summary>
        /// ��ȡ������֡��
        /// </summary>
        public int FrameRote
        {
            get
            {
                return frameRote;
            }
            set
            {
                frameRote = value;
                Application.targetFrameRate = value;
            }
        }
        
        private void Start()
        {
            Application.targetFrameRate = frameRote;
            
            Application.runInBackground = runInBackgroun;
            
            Screen.sleepTimeout = neverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
        }

    }

}
