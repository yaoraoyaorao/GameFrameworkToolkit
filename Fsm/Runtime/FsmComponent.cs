using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class FsmComponent : MonoBehaviour
    {
        private Dictionary<string, FsmBase> m_Fsms;
        private List<FsmBase> m_TempFsms;

        public int Count
        {
            get { return m_Fsms.Count; }
        }

        private void Awake()
        {
            m_Fsms = new Dictionary<string, FsmBase>();
            m_TempFsms = new List<FsmBase>();
        }

        private void Update()
        {
            m_TempFsms.Clear();
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (KeyValuePair<string, FsmBase> fsm in m_Fsms)
            {
                m_TempFsms.Add(fsm.Value);
            }

            foreach (FsmBase fsm in m_TempFsms)
            {
                if (fsm.IsDestroyed)
                {
                    continue;
                }

                fsm.Update(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            foreach (KeyValuePair<string, FsmBase> fsm in m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            m_Fsms.Clear();
            m_TempFsms.Clear();
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(GetFullName(typeof(T)));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(GetFullName(ownerType));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(GetFullName(typeof(T), name));
        }

        /// <summary>
        /// ����Ƿ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>�Ƿ��������״̬����</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalHasFsm(GetFullName(ownerType, name));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public IFsm<T> GetFsm<T>() where T : class
        {
            return (IFsm<T>)InternalGetFsm(GetFullName(typeof(T)));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public FsmBase GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(GetFullName(ownerType));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public IFsm<T> GetFsm<T>(string name) where T : class
        {
            return (IFsm<T>)InternalGetFsm(GetFullName(typeof(T), name));
        }

        /// <summary>
        /// ��ȡ����״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">����״̬�����ơ�</param>
        /// <returns>Ҫ��ȡ������״̬����</returns>
        public FsmBase GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalGetFsm(GetFullName(ownerType, name));
        }

        /// <summary>
        /// ��ȡ��������״̬����
        /// </summary>
        /// <returns>��������״̬����</returns>
        public FsmBase[] GetAllFsms()
        {
            int index = 0;
            FsmBase[] results = new FsmBase[m_Fsms.Count];
            foreach (KeyValuePair<string, FsmBase> fsm in m_Fsms)
            {
                results[index++] = fsm.Value;
            }

            return results;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">����״̬�����ơ�</param>
        /// <param name="owner">����״̬�������ߡ�</param>
        /// <param name="states">����״̬��״̬���ϡ�</param>
        /// <returns>Ҫ����������״̬����</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            string typeNamePair = GetFullName(typeof(T), name);
            if (HasFsm<T>(name))
            {
                throw new Exception(string.Format("Already exist FSM '{0}'.", typeNamePair));
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(typeNamePair, fsm);
            return fsm;
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(GetFullName(typeof(T)));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(GetFullName(typeof(T), name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="ownerType">����״̬�����������͡�</param>
        /// <param name="name">Ҫ���ٵ�����״̬�����ơ�</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new Exception("Owner type is invalid.");
            }

            return InternalDestroyFsm(GetFullName(ownerType, name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <typeparam name="T">����״̬�����������͡�</typeparam>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid.");
            }

            return InternalDestroyFsm(GetFullName(typeof(T), fsm.Name));
        }

        /// <summary>
        /// ��������״̬����
        /// </summary>
        /// <param name="fsm">Ҫ���ٵ�����״̬����</param>
        /// <returns>�Ƿ���������״̬���ɹ���</returns>
        public bool DestroyFsm(FsmBase fsm)
        {
            if (fsm == null)
            {
                throw new Exception("FSM is invalid.");
            }

            return InternalDestroyFsm(GetFullName(fsm.OwnerType, fsm.Name));
        }

        private bool InternalHasFsm(string typeNamePair)
        {
            return m_Fsms.ContainsKey(typeNamePair);
        }

        private FsmBase InternalGetFsm(string typeNamePair)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeNamePair, out fsm))
            {
                return fsm;
            }

            return null;
        }

        private bool InternalDestroyFsm(string typeNamePair)
        {
            FsmBase fsm = null;
            if (m_Fsms.TryGetValue(typeNamePair, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(typeNamePair);
            }

            return false;
        }

        private string GetFullName(Type type, string name = "")
        {
            return !string.IsNullOrEmpty(name) ?
                   type.FullName + "." + name :
                   type.FullName;
        }

    }
}
