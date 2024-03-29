﻿using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Respository
{
    public class CompanyRespository : ICompanyRespository
    {
        //dapper
        IDbConnection db;
        public CompanyRespository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            //returns the CompanyId 
            var sql = @"INSERT INTO Companies(Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);
                        SELECT CAST(SCOPE_IDENTITY() AS int);";
            var id = db.Query<int>(sql, company).Single();
            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId";
            return db.Query<Company>(sql, new { @CompanyId = id }).Single();
        }

        public List<Company> GetAll()
        {
            var sql = "SELECT * FROM Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @id";
            //DynamicParameters compId = new DynamicParameters();
            //compId.Add("@id",id);
            db.Execute(sql, new { id});
        }

        public Company Update(Company company)
        {
            //parameter name must be same as property name if company obj variable is used
            var sql = @"UPDATE Companies SET Name = @Name, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode
                        WHERE CompanyId = @CompanyId ";
            db.Execute(sql, company);
            return company;
        }
    }
}
