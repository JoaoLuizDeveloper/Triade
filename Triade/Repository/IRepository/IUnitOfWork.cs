using System;

namespace LubyTechAPI.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IDeveloperRepository Developer { get; }
        IProjectRepository Project { get; }
    }
}
