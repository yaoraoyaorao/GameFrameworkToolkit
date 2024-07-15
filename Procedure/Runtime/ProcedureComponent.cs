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
        /// 获取当前流程。
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
        /// 获取当前流程持续时间。
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
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start<T>();
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        public void StartProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(procedureType);
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState<T>();
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.GetState<T>();
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
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
