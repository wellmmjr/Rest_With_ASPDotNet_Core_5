using Microsoft.EntityFrameworkCore;
using RestWithASPNet.Model.Base;
using RestWithASPNet.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNet.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {

        protected MySQLContext _context;

        private DbSet<T> dataSet;

        public GenericRepository(MySQLContext context)
        {
            _context = context;
            dataSet = _context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                dataSet.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public T Update(T item)
        {
            var result = dataSet.SingleOrDefault(p => p.Id.Equals(item.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                return null;
            }
        }

        public List<T> FindAll()
        {
            return dataSet.ToList();
        
        }

        public T FindById(long id)
        {
            return dataSet.SingleOrDefault(i => i.Id.Equals(id));
        }

        public void Delete(long id)
        {
            var result = dataSet.SingleOrDefault(p => p.Id.Equals(id));

            if (result != null)
            {
                try
                {
                    dataSet.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }


        public bool Exists(long id)
        {
            return dataSet.Any(p => p.Id.Equals(id));
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return dataSet.FromSqlRaw<T>(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }
            return int.Parse(result);
        }
    }
}
