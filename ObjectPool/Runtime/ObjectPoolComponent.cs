using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class ObjectPoolComponent : MonoBehaviour
    {
        private ObjectPoolManager m_ObjectPoolManager = null;


        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        public int Count
        {
            get
            {
                return m_ObjectPoolManager.Count;
            }
        }

        private void Start()
        {
            m_ObjectPoolManager = new ObjectPoolManager();

            if (m_ObjectPoolManager == null)
            {
                Debug.Log("Object pool manager is invalid.");
                return;
            }
        }

        private void Update()
        {
            if (m_ObjectPoolManager != null)
            {
                m_ObjectPoolManager.Update(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            m_ObjectPoolManager.Shutdown();
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.HasObjectPool<T>();
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Type objectType)
        {
            return m_ObjectPoolManager.HasObjectPool(objectType);
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool<T>(string name) where T : ObjectBase
        {
            return m_ObjectPoolManager.HasObjectPool<T>(name);
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Type objectType, string name)
        {
            return m_ObjectPoolManager.HasObjectPool(objectType, name);
        }

        /// <summary>
        /// ����Ƿ���ڶ���ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>�Ƿ���ڶ���ء�</returns>
        public bool HasObjectPool(Predicate<ObjectPoolBase> condition)
        {
            return m_ObjectPoolManager.HasObjectPool(condition);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.GetObjectPool<T>();
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            return m_ObjectPoolManager.GetObjectPool(objectType);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            return m_ObjectPoolManager.GetObjectPool<T>(name);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Type objectType, string name)
        {
            return m_ObjectPoolManager.GetObjectPool(objectType, name);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase GetObjectPool(Predicate<ObjectPoolBase> condition)
        {
            return m_ObjectPoolManager.GetObjectPool(condition);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <returns>Ҫ��ȡ�Ķ���ء�</returns>
        public ObjectPoolBase[] GetObjectPools(Predicate<ObjectPoolBase> condition)
        {
            return m_ObjectPoolManager.GetObjectPools(condition);
        }

        /// <summary>
        /// ��ȡ����ء�
        /// </summary>
        /// <param name="condition">Ҫ����������</param>
        /// <param name="results">Ҫ��ȡ�Ķ���ء�</param>
        public void GetObjectPools(Predicate<ObjectPoolBase> condition, List<ObjectPoolBase> results)
        {
            m_ObjectPoolManager.GetObjectPools(condition, results);
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        public ObjectPoolBase[] GetAllObjectPools()
        {
            return m_ObjectPoolManager.GetAllObjectPools();
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="results">���ж���ء�</param>
        public void GetAllObjectPools(List<ObjectPoolBase> results)
        {
            m_ObjectPoolManager.GetAllObjectPools(results);
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <returns>���ж���ء�</returns>
        public ObjectPoolBase[] GetAllObjectPools(bool sort)
        {
            return m_ObjectPoolManager.GetAllObjectPools(sort);
        }

        /// <summary>
        /// ��ȡ���ж���ء�
        /// </summary>
        /// <param name="sort">�Ƿ���ݶ���ص����ȼ�����</param>
        /// <param name="results">���ж���ء�</param>
        public void GetAllObjectPools(bool sort, List<ObjectPoolBase> results)
        {
            m_ObjectPoolManager.GetAllObjectPools(sort, results);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>();
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>(name, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateSingleSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool(objectType, name, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>();
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, capacity);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, capacity, expireTime);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, capacity, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>(string name, float autoReleaseInterval, int capacity, float expireTime, int priority) where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>(name, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���������λ�ȡ�Ķ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">��������ơ�</param>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ��������</param>
        /// <param name="capacity">����ص�������</param>
        /// <param name="expireTime">����ض������������</param>
        /// <param name="priority">����ص����ȼ���</param>
        /// <returns>Ҫ�����������λ�ȡ�Ķ���ء�</returns>
        public ObjectPoolBase CreateMultiSpawnObjectPool(Type objectType, string name, float autoReleaseInterval, int capacity, float expireTime, int priority)
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool(objectType, name, autoReleaseInterval, capacity, expireTime, priority);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.DestroyObjectPool<T>();
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(Type objectType)
        {
            return m_ObjectPoolManager.DestroyObjectPool(objectType);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            return m_ObjectPoolManager.DestroyObjectPool<T>(name);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectType">�������͡�</param>
        /// <param name="name">Ҫ���ٵĶ�������ơ�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(Type objectType, string name)
        {
            return m_ObjectPoolManager.DestroyObjectPool(objectType, name);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <typeparam name="T">�������͡�</typeparam>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool<T>(IObjectPool<T> objectPool) where T : ObjectBase
        {
            return m_ObjectPoolManager.DestroyObjectPool(objectPool);
        }

        /// <summary>
        /// ���ٶ���ء�
        /// </summary>
        /// <param name="objectPool">Ҫ���ٵĶ���ء�</param>
        /// <returns>�Ƿ����ٶ���سɹ���</returns>
        public bool DestroyObjectPool(ObjectPoolBase objectPool)
        {
            return m_ObjectPoolManager.DestroyObjectPool(objectPool);
        }

        /// <summary>
        /// �ͷŶ�����еĿ��ͷŶ���
        /// </summary>
        public void Release()
        {
            Debug.Log("Object pool release...");
            m_ObjectPoolManager.Release();
        }

        /// <summary>
        /// �ͷŶ�����е�����δʹ�ö���
        /// </summary>
        public void ReleaseAllUnused()
        {
            Debug.Log("Object pool release all unused...");
            m_ObjectPoolManager.ReleaseAllUnused();
        }
    }
}
