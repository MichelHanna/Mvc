// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    /// <summary>
    /// A default implementation of <see cref="IModelMetadataProvider"/>.
    /// </summary>
    internal class ModelExpressionProvider : IModelExpressionProvider
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ExpressionHelper _expressionHelper;

        /// <summary>
        /// Creates a  new <see cref="ModelExpressionProvider"/>.
        /// </summary>
        /// <param name="modelMetadataProvider">The <see cref="IModelMetadataProvider"/>.</param>
        /// <param name="expressionHelper">The <see cref="ExpressionHelper"/>.</param>
        public ModelExpressionProvider(
            IModelMetadataProvider modelMetadataProvider,
            ExpressionHelper expressionHelper)
        {
            if (modelMetadataProvider == null)
            {
                throw new ArgumentNullException(nameof(modelMetadataProvider));
            }

            if (expressionHelper == null)
            {
                throw new ArgumentNullException(nameof(expressionHelper));
            }

            _modelMetadataProvider = modelMetadataProvider;
            _expressionHelper = expressionHelper;
        }

        /// <inheritdoc />
        public ModelExpression CreateModelExpression<TModel, TValue>(
            ViewDataDictionary<TModel> viewData,
            Expression<Func<TModel, TValue>> expression)
        {
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var name = _expressionHelper.GetExpressionText(expression);
            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, viewData, _modelMetadataProvider);
            if (modelExplorer == null)
            {
                throw new InvalidOperationException(
                    Resources.FormatCreateModelExpression_NullModelMetadata(nameof(IModelMetadataProvider), name));
            }

            return new ModelExpression(name, modelExplorer);
        }

        /// <inheritdoc />
        public ModelExpression CreateModelExpression<TModel>(
            ViewDataDictionary<TModel> viewData,
            string expression)
        {
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var modelExplorer = ExpressionMetadataProvider.FromStringExpression(expression, viewData, _modelMetadataProvider);
            if (modelExplorer == null)
            {
                throw new InvalidOperationException(
                    Resources.FormatCreateModelExpression_NullModelMetadata(nameof(IModelMetadataProvider), expression));
            }

            return new ModelExpression(expression, modelExplorer);
        }
    }
}
