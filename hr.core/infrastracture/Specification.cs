using System;
using System.Linq.Expressions;

namespace hr.core.infrastracture {

    /// <summary>
    /// https://enterprisecraftsmanship.com/posts/specification-pattern-c-implementation/
    /// </summary>
    public abstract class Specification<T> {
        public abstract Expression<Func<T, bool>> toExpression();

        public bool isSatisfiedBy(T entity) {
            Func<T, bool> predicate = this.toExpression().Compile();
            return predicate(entity);
        }

        public AndSpecification<T> And(Specification<T> specification) {
            return new AndSpecification<T>(this, specification);
        }

        public OrSpecification<T> Or(Specification<T> specification) {
            return new OrSpecification<T>(this, specification);
        }
    }

    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right) {
            this._left = left;
            this._right = right;
        }

        public override Expression<Func<T, bool>> toExpression()
        {
            Expression<Func<T, bool>> leftExpression = this._left.toExpression();
            Expression<Func<T, bool>> rightExpression = this._right.toExpression();

            BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, Expression.Invoke(rightExpression, leftExpression.Parameters[0]));
            // Console.WriteLine(andExpression.ToString());
            return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters);
        }
    }

    public class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right) {
            this._left = left;
            this._right = right;
        }

        public override Expression<Func<T, bool>> toExpression()
        {
            Expression<Func<T, bool>> leftExpression = this._left.toExpression();
            Expression<Func<T, bool>> rightExpression = this._right.toExpression();

            BinaryExpression orExpression = Expression.Or(leftExpression.Body, Expression.Invoke(rightExpression, leftExpression.Parameters[0]));
            Console.WriteLine(orExpression.ToString());
            return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters);
        }
    }

}