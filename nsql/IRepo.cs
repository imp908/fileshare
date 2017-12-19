using System;
using System.Collections.Generic;
using IOrientObjects;
using IQueryManagers;

namespace IRepos
{
    public interface IRepo_v1
    {

    string Add(IorientDefaultObject obj_);
    string Add(ITypeToken db_name, ITypeToken command_type);
    string Add(ITypeToken rest_command_, ITypeToken dbName_, ITypeToken type_);
    string Add(IOrientEdge obj_, IOrientVertex from, IOrientVertex to);
    string Add(IOrientEdge obj_, ITypeToken from, ITypeToken to);
    void changeAuthCredentials(string Login, string Password);
    string Delete(Type type_, ITypeToken condition_);
    string Delete(Type type_, IOrientEntity from_, IOrientEntity to_, ITypeToken condition_);
    string Function(ITypeToken functionName, List<ITypeToken> params_);
    string Select(Type object_, ITypeToken condition_);
    IEnumerable<T> Select<T>(string command_) where T : class;
    IEnumerable<T> Select<T>(Type object_, ITypeToken condition_) where T : class;

    }
    
    
    
}