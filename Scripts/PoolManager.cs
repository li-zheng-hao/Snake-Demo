using System;
using System.Collections.Generic;
using UnityEngine;
namespace PoolManager
{
    //单元
    public interface Pool_Unit
    {
        //当前对象的状态
        Pool_UnitState state();
        //设置父类
        void setParentList(object parentList);
        //回收
        void restore();
    }
    public enum Pool_Type
    {
        //空闲
        Idle,
        //使用
        Work
    }
    public class Pool_UnitState
    {
        public Pool_Type InPool
        {
            get;
            set;
        }
    }
    //单元链表
    public abstract class Pool_UnitList<T> where T : class, Pool_Unit
    {
        //创建的单元的模板
        protected object m_template;
        //闲置单元链表
        protected List<T> m_idleList;
        //使用单元链表
        protected List<T> m_workList;
        //已经存在的单元个数
        protected int m_createdNum = 0;
        public Pool_UnitList()
        {
            m_idleList = new List<T>();
            m_workList = new List<T>();
        }

        /// <summary>
        /// 获取一个闲置的单元，如果不存在则创建一个新的
        /// </summary>
        /// <returns>闲置单元</returns>
        public virtual T takeUnit<UT>() where UT : T
        {
            T unit;
            //如果闲置单元中存在一个单元
            if (m_idleList.Count > 0)
            {
                unit = m_idleList[0];
                m_idleList.RemoveAt(0);
            }
            //不存在闲置的单元
            else
            {
                unit = createNewUnit<UT>();
                unit.setParentList(this);
                m_createdNum++;
            }
            //加入到使用的链表
            m_workList.Add(unit);
            //设置状态为使用
            unit.state().InPool = Pool_Type.Work;
            OnUnitChangePool(unit);
            return unit;
        }
        /// <summary>
        /// 归还某个单元
        /// </summary>
        /// <param name="unit">单元</param>
        public virtual void restoreUnit(T unit)
        {
            if (unit != null && unit.state().InPool == Pool_Type.Work)
            {
                m_workList.Remove(unit);
                m_idleList.Add(unit);
                unit.state().InPool = Pool_Type.Idle;
                OnUnitChangePool(unit);
            }
        }
        /// <summary>
        /// 设置模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        public void setTemplate(object template)
        {
            m_template = template;
        }
        /// <summary>
        /// 单元所处的链表发生改变
        /// </summary>
        /// <param name="unit"></param>
        protected abstract void OnUnitChangePool(T unit);
        /// <summary>
        /// 创建新的单元
        /// </summary>
        /// <typeparam name="UT"></typeparam>
        /// <returns></returns>
        protected abstract T createNewUnit<UT>() where UT : T;
    }

    public abstract class Pool_Base<UnitType, UnitList> : MonoBehaviour
        where UnitType : class, Pool_Unit
        where UnitList : Pool_UnitList<UnitType>//, new()
    {
        /// <summary>
        /// 缓冲池，按类型存放各自分类列表
        /// </summary>
        private Dictionary<Type, UnitList> m_poolTale = new Dictionary<Type, UnitList>();

        

        /// <summary>
        /// 获取一个空闲的单元
        /// </summary>
        public T takeUnit<T>() where T : class, UnitType
        {
            UnitList list = getList<T>();
            return list.takeUnit<T>() as T;
        }

        /// <summary>
        /// 在缓冲池中获取指定单元类型的列表，
        /// 如果该单元类型不存在，则立刻创建。
        /// </summary>
        /// <typeparam name="T">单元类型</typeparam>
        /// <returns>单元列表</returns>
        public UnitList getList<T>() where T : UnitType
        {
            var t = typeof(T);
            UnitList list = null;
            m_poolTale.TryGetValue(t, out list);
            if (list == null)
            {
                list = createNewUnitList<T>();
                m_poolTale.Add(t, list);
            }
            return list;
        }
        protected abstract UnitList createNewUnitList<UT>() where UT : UnitType;
    }

    public class Pool_Comp : Pool_Base<Pooled_BehaviorUnit, Pool_UnitList_Comp>
    {
        [SerializeField]
        [Tooltip("运行父节点")]
        protected Transform m_work;
        [SerializeField]
        [Tooltip("闲置父节点")]
        protected Transform m_idle;

        
        public virtual void Awake()
        {
            if (m_work == null)
            {
                m_work = CompUtil.Create(transform, "work");
            }
            if (m_idle == null)
            {
                m_idle = CompUtil.Create(transform, "idle");
                m_idle.gameObject.SetActive(false);
            }
        }
        public void OnUnitChangePool(Pooled_BehaviorUnit unit)
        {
            if (unit != null)
            {
                var inPool = unit.state().InPool;
                if (inPool == Pool_Type.Idle)
                {
                    unit.transform.SetParent(m_idle);
                }
                else if (inPool == Pool_Type.Work)
                {
                    unit.transform.SetParent(m_work);
                }
            }
        }
        protected override Pool_UnitList_Comp createNewUnitList<UT>()
        {
            Pool_UnitList_Comp list = new Pool_UnitList_Comp();
            list.setPool(this);
            return list;
        }


    }

    public abstract class Pooled_BehaviorUnit : MonoBehaviour, Pool_Unit
    {
        //单元状态对象
        protected Pool_UnitState m_unitState = new Pool_UnitState();
        //父列表对象
        Pool_UnitList<Pooled_BehaviorUnit> m_parentList;
        /// <summary>
        /// 返回一个单元状态，用于控制当前单元的闲置、工作状态
        /// </summary>
        /// <returns>单元状态</returns>
        public virtual Pool_UnitState state()
        {
            return m_unitState;
        }
        /// <summary>
        /// 接受父列表对象的设置
        /// </summary>
        /// <param name="parentList">父列表对象</param>
        public virtual void setParentList(object parentList)
        {
            m_parentList = parentList as Pool_UnitList<Pooled_BehaviorUnit>;
        }
        /// <summary>
        /// 归还自己，即将自己回收以便再利用
        /// </summary>
        public virtual void restore()
        {
            if (m_parentList != null)
            {
                m_parentList.restoreUnit(this);
            }
        }

    }

    public class Pool_UnitList_Comp : Pool_UnitList<Pooled_BehaviorUnit>
    {
        protected Pool_Comp m_pool;
        public void setPool(Pool_Comp pool)
        {
            m_pool = pool;
        }
        protected override Pooled_BehaviorUnit createNewUnit<UT>()
        {
            GameObject result_go = null;
            if (m_template != null && m_template is GameObject)
            {
                result_go = GameObject.Instantiate((GameObject)m_template);
            }
            else
            {
                result_go = new GameObject();
                result_go.name = typeof(UT).Name;
            }
            result_go.name = result_go.name + "_" + m_createdNum;
            UT comp = result_go.GetComponent<UT>();
            if (comp == null)
            {
                comp = result_go.AddComponent<UT>();
            }
            //comp.DoInit();
            return comp;
        }

        protected override void OnUnitChangePool(Pooled_BehaviorUnit unit)
        {
            if (m_pool != null)
            {
                m_pool.OnUnitChangePool(unit);
            }
        }
    }

    public static class CompUtil
    {
        /// <summary>
        /// 在指定节点的下方创建一个新的GameObject
        /// </summary>
        /// <param name="_transform">父节点</param>
        /// <param name="name">名称</param>
        /// <returns>新节点变换对象</returns>
        public static Transform Create(Transform _transform, string name)
        {
            GameObject goNew = new GameObject(name);
            Transform trNew = goNew.transform;
            if (_transform != null)
            {
                trNew.SetParent(_transform);
            }
            trNew.localPosition = Vector3.zero;
            trNew.localScale = Vector3.one;
            trNew.localRotation = Quaternion.identity;
            goNew.name = name;
            return trNew;
        }
    }

}

