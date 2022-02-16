using System;
using System.Collections.Generic;
using System.Text;
using LubyTechAPI.Data;

namespace Triade.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Developer = new DeveloperRepository(_db);
            Project = new ProjectRepository(_db);
        }

        public IDeveloperRepository Developer { get; private set; }
        public IProjectRepository Project { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
