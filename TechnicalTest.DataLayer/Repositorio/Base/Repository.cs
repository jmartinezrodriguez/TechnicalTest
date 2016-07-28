
using TechnicalTest.DataLayer;
using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using TechnicalTest.CrossLayer;

namespace TechnicalTest.DataLayer
{
    /// <summary>
    /// Default base class for repostories. This generic repository
    /// is a default implementation of <see cref="IRepository{TEntity}" />
    /// and your specific repositories can inherit from this base class so automatically will get default implementation.
    /// IMPORTANT: Using this Base Repository class IS NOT mandatory. It is just a useful base class:
    /// You could also decide that you do not want to use this base Repository class, because sometimes you don't want a
    /// specific Repository getting all these features and it might be wrong for a specific Repository.
    /// For instance, you could want just read-only data methods for your Repository, etc.
    /// in that case, just simply do not use this base class on your Repository.
    /// </summary>
    /// <typeparam name="TEntity">Type of elements in repostory</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IObjectWithChangeTracker
    {
        #region Members

        /// <summary>
        /// The _ current uo W
        /// </summary>
        private IQueryableUnitOfWork _CurrentUoW;

        #endregion Members

        #region Constructor

        /// <summary>
        /// Create a new instance of Repository
        /// </summary>
        /// <param name="unitOfWork">A unit of work for this repository</param>
        /// <exception cref="System.ArgumentNullException">unitOfWork</exception>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            //check preconditions
            if (unitOfWork == (IQueryableUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork", Messages.exception_ContainerCannotBeNull);

            //set internal values
            _CurrentUoW = unitOfWork;
        }

        #endregion Constructor

        #region IRepository<TEntity> Members

        /// <summary>
        /// Establece o retorna la propiedad Total registros
        /// </summary>
        /// <value>The total registros.</value>
        public int TotalRegistros { get; set; }

        /// <summary>
        /// Return a unit of work in this repository
        /// </summary>
        /// <value>The unit of work.</value>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _CurrentUoW as IUnitOfWork;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public virtual void Add(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);

            //add object to IObjectSet for this type

            //really for STE you have two options, addobject and
            //call to ApplyChanges in this objetSet. After
            //review discussion feedback in our codeplex project
            //ApplyChanges is the best option because solve problems in
            //many to many associations and AddObject method

            if (item.ChangeTracker != null
                &&
                item.ChangeTracker.State == ObjectState.Added)
            {
                _CurrentUoW.RegisterChanges<TEntity>(item);
            }
            else
                throw new InvalidOperationException(Messages.exception_ChangeTrackerIsNullOrStateIsNOK);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>`0[][].</returns>
        public virtual TEntity[] GetAll()
        {
            //Create IObjectSet and perform query
            return (CreateSet()).AsEnumerable<TEntity>().ToArray();
        }

        /// <summary>
        /// Gets the filtered elements.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>`0[][].</returns>
        /// <exception cref="System.ArgumentNullException">filter</exception>
        public virtual TEntity[] GetFilteredElements(Expression<Func<TEntity, bool>> filter)
        {
            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter",   Messages.exception_FilterCannotBeNull);

            //Create IObjectSet and perform query
            return CreateSet().Where(filter)
                                    .ToArray();
        }

        /// <summary>
        /// Gets the filtered elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>`0[][].</returns>
        /// <exception cref="System.ArgumentNullException">
        /// filter
        /// or
        /// orderByExpression
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual TEntity[] GetFilteredElements<T>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, T>> orderByExpression, bool ascending)
        {
            //Checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter", Messages.exception_FilterCannotBeNull);

            if (orderByExpression == (Expression<Func<TEntity, T>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);

            //Create IObjectSet for this type and perform query
            IObjectSet<TEntity> objectSet = CreateSet();

            return (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .ToArray()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .ToArray();
        }

        /// <summary>
        /// Gets the filtered paged elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>`0[][].</returns>
        /// <exception cref="System.ArgumentNullException">
        /// filter
        /// or
        /// orderByExpression
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// pageIndex
        /// or
        /// pageCount
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual TEntity[] GetFilteredPagedElements<T>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, T>> orderByExpression, bool ascending)
        {
            //checking query arguments
            if (filter == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("filter", Messages.exception_FilterCannotBeNull);

            if (pageIndex < 0)
                throw new ArgumentException(Messages.exception_InvalidPageIndex, "pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException(Messages.exception_InvalidPageCount, "pageCount");

            if (orderByExpression == (Expression<Func<TEntity, T>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);

            //Create IObjectSet for this particular type and query this
            IObjectSet<TEntity> objectSet = CreateSet();

            return (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToArray()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToArray();
        }

        /// <summary>
        /// Get all elements of type {TEntity} in repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        /// <exception cref="System.ArgumentException">pageIndex
        /// or
        /// pageCount</exception>
        /// <exception cref="System.ArgumentNullException">orderByExpression</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public virtual TEntity[] GetPagedElements<T>(int pageIndex, int pageCount, Expression<Func<TEntity, T>> orderByExpression, bool ascending)
        {
            //checking query arguments
            if (pageIndex < 0)
                throw new ArgumentException(Messages.exception_InvalidPageIndex, "pageIndex");

            if (pageCount <= 0)
                throw new ArgumentException(Messages.exception_InvalidPageCount, "pageCount");

            if (orderByExpression == (Expression<Func<TEntity, T>>)null)
                throw new ArgumentNullException("orderByExpression", Messages.exception_OrderByExpressionCannotBeNull);

            //Create IObjectSet for this particular type and query this
            IObjectSet<TEntity> objectSet = CreateSet();

            return (ascending)
                                ?
                                    objectSet
                                     .OrderBy(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToArray()
                                :
                                    objectSet
                                     .OrderByDescending(orderByExpression)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount)
                                     .ToArray();
        }

        /// <summary>
        /// Modifies the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void Modify(TEntity item)
        {
            //check arguments
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);

            //Set modifed state if change tracker is enabled and state is not deleted
            if (item.ChangeTracker != null
                &&
                ((item.ChangeTracker.State & ObjectState.Deleted) != ObjectState.Deleted)
               )
            {
                item.MarkAsModified();
            }
            //apply changes for item object
            _CurrentUoW.RegisterChanges(item);
        }

        /// <summary>
        /// Modifies the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="changedPropertyNames">The changed property names.</param>
        public virtual void Modify(TEntity item, params string[] changedPropertyNames)
        {
            //ObjectContext context = (ObjectContext)_CurrentUoW;
            //ObjectStateEntry stateEntry = context.ObjectStateManager.GetObjectStateEntry(item);
            //foreach (var propertyName in changedPropertyNames)
            //{
            //    // If we can't find the property, this line wil throw an exception,
            //    //which is good as we want to know about it
            //    stateEntry.SetModifiedProperty(propertyName);
            //}
            Modify(item);
        }

        /// <summary>
        /// Registers the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void RegisterItem(TEntity item)
        {
            if (item == (TEntity)null)
                throw new ArgumentNullException("item");

            (CreateSet()).Attach(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public virtual void Remove(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", Messages.exception_ItemArgumentIsNull);

            IObjectSet<TEntity> objectSet = CreateSet();

            //Attach object to unit of work and delete this
            // this is valid only if T is a type in model
            objectSet.Attach(item);

            //delete object to IObjectSet for this type
            objectSet.DeleteObject(item);
        }

        #endregion IRepository<TEntity> Members

        #region Private Methods

        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <returns>IObjectSet{`0}.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        private IObjectSet<TEntity> CreateSet()
        {
            if (_CurrentUoW != (IUnitOfWork)null)
            {
                IObjectSet<TEntity> objectSet = _CurrentUoW.CreateSet<TEntity>();

                //set merge option to underlying ObjectQuery

                ObjectQuery<TEntity> query = objectSet as ObjectQuery<TEntity>;

                if (query != null) // check if this objectset is not in memory object set for testing
                    query.MergeOption = MergeOption.NoTracking;

                return objectSet;
            }
            else
                throw new InvalidOperationException(Messages.exception_ContainerCannotBeNull);
        }

        #endregion Private Methods
    }
}