using System;
using System.Collections;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    [DefaultExecutionOrder(-90)]
    public class ProcedureComponent: MonoBehaviour
    {
        [SerializeField]
        private string[] m_AvailableProcedureTypeNames = null;

        [SerializeField]
        private string m_EntranceProcedureTypeName = null;

        private FsmComponent m_FsmComponent;
        private IFsm<ProcedureComponent> m_ProcedureFsm;
        private ProcedureBase m_EntranceProcedure = null;

        /// <summary>
        /// ��ȡ��ǰ���̡�
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (m_ProcedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return (ProcedureBase)m_ProcedureFsm.CurrentState;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���̳���ʱ�䡣
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (m_ProcedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return m_ProcedureFsm.CurrentStateTime;
            }
        }

        private void Awake()
        {
            m_FsmComponent = GameObject.FindObjectOfType<FsmComponent>();

            if (m_FsmComponent == null)
            {
                Debug.LogError("FsmComponent is invalid");
            }

            StartCoroutine(Initialize());
        }

        private void OnDestroy()
        {
            if (m_FsmComponent != null)
            {
                if (m_ProcedureFsm != null)
                {
                    m_FsmComponent.DestroyFsm(m_ProcedureFsm);
                    m_ProcedureFsm = null;
                }

                m_FsmComponent = null;
            }
        }

        public IEnumerator Initialize()
        {
            int count = m_AvailableProcedureTypeNames.Length;

            ProcedureBase[] procedures = new ProcedureBase[count];

            for (int i = 0; i < count; i++)
            {
                string procedureTypeName = m_AvailableProcedureTypeNames[i];

                Type procedureType = AssemblyUtility.GetType(procedureTypeName);

                if (procedureType == null)
                {
                    Debug.LogError($"Can not find procedure type '{procedureTypeName}'.");
                    yield break;
                }

                procedures[i] = (ProcedureBase)Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Debug.LogError($"Can not create procedure instance '{procedureTypeName}'.");
                    yield break;
                }

                if (m_EntranceProcedureTypeName == procedureTypeName)
                {
                    m_EntranceProcedure = procedures[i];
                }
            }

            if (m_EntranceProcedure == null)
            {
                Debug.LogError("Entrance procedure is invalid.");
                yield break;
            }

            m_ProcedureFsm = m_FsmComponent.CreateFsm<ProcedureComponent>(this, procedures);

            yield return new WaitForEndOfFrame();

            StartProcedure(m_EntranceProcedure.GetType());
        }

        /// <summary>
        /// ��ʼ���̡�
        /// </summary>
        /// <typeparam name="T">Ҫ��ʼ���������͡�</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start<T>();
        }

        /// <summary>
        /// ��ʼ���̡�
        /// </summary>
        /// <param name="procedureType">Ҫ��ʼ���������͡�</param>
        public void StartProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(procedureType);
        }

        /// <summary>
        /// �Ƿ�������̡�
        /// </summary>
        /// <typeparam name="T">Ҫ�����������͡�</typeparam>
        /// <returns>�Ƿ�������̡�</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState<T>();
        }

        /// <summary>
        /// �Ƿ�������̡�
        /// </summary>
        /// <param name="procedureType">Ҫ�����������͡�</param>
        /// <returns>�Ƿ�������̡�</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState(procedureType);
        }

        /// <summary>
        /// ��ȡ���̡�
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ���������͡�</typeparam>
        /// <returns>Ҫ��ȡ�����̡�</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.GetState<T>();
        }

        /// <summary>
        /// ��ȡ���̡�
        /// </summary>
        /// <param name="procedureType">Ҫ��ȡ���������͡�</param>
        /// <returns>Ҫ��ȡ�����̡�</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return (ProcedureBase)m_ProcedureFsm.GetState(procedureType);
        }
    }
}
