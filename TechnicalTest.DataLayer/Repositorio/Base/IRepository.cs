﻿using TechnicalTest.DataLayer;

//===================================================================================
// Microsoft Developer & Platform Evangelism
//===================================================================================
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license,
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
using System;
using System.Linq.Expressions;

namespace TechnicalTest.DataLayer
{
    /// <summary>
    /// Base interface for implement a "Repository Pattern", for
    /// more information about this pattern see http://martinfowler.com/eaaCatalog/repository.html
    /// or http://blogs.msdn.com/adonet/archive/2009/06/16/using-repository-and-unit-of-work-patterns-with-entity-framework-4-0.aspx
    /// </summary>
    /// <remarks>
    /// Indeed, one might think that IObjectSet is already a generic repository and therefore
    /// would not need this item. Using this interface allows us to ensure PI principle
    /// within our domain model
    /// </remarks>
    /// <typeparam name="TEntity">Type of entity for this repository </typeparam>
    internal interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TEntity item);

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        ///Register entity into this repository, really in UnitOfWork.
        ///In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void RegisterItem(TEntity item);

        /// <summary>
        /// Sets modified entity into the repository.
        /// When calling Commit() method in UnitOfWork
        /// these changes will be saved into the storage
        /// <remarks>
        /// Internally this method always calls Repository.Attach() and Context.SetChanges()
        /// </remarks>
        /// </summary>
        /// <param name="item">Item with changes</param>
        void Modify(TEntity item);

        /// <summary>
        /// Sets modified entity into the repository.
        /// This method only modify changed properties.
        /// </summary>
        /// <param name="item">Item with changes</param>
        /// <param name="changedPropertyNames">Changed properties list</param>
        void Modify(TEntity item, params string[] changedPropertyNames);

        /// <summary>
        /// Get all elements of type {TEntity} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        TEntity[] GetAll();

        ///// <summary>
        ///// Get all elements of type {TEntity} that matching a
        ///// Specification <paramref name="specification"/>
        ///// </summary>
        ///// <param name="specification">Specification that result meet</param>
        ///// <returns></returns>
        //TEntity[] GetBySpec(ISpecification<TEntity> specification);

        /// <summary>
        /// Get all elements of type {TEntity} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        TEntity[] GetPagedElements<T>(int pageIndex, int pageCount, Expression<Func<TEntity, T>> orderByExpression, bool ascending);

        /// <summary>
        /// Get elements of type {TEntity} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        TEntity[] GetFilteredElements(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get all elements of type {TEntity} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <param name="pageIndex">Index of page</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        TEntity[] GetFilteredPagedElements<T>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, T>> orderByExpression, bool ascending);
    }
}