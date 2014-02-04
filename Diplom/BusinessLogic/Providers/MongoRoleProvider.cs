﻿using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Hosting;
using System.Web.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Investmogilev.Infrastructure.BusinessLogic.Providers
{
    public class MongoRoleProvider : RoleProvider
    {
        private MongoCollection _rolesMongoCollection;
        private MongoCollection _usersInRolesMongoCollection;

        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            // Make sure each role exists
            foreach (var roleName in roleNames)
            {
                if (!this.RoleExists(roleName))
                {
                    throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
                }
            }

            foreach (var username in usernames)
            {
                var membershipUser = Membership.GetUser(username);

                if (membershipUser == null)
                {
                    throw new ProviderException(string.Format("The user '{0}' was not found.", username));
                }

                foreach (var roleName in roleNames)
                {
                    if (this.IsUserInRole(username, roleName))
                    {
                        throw new ProviderException(string.Format("The user '{0}' is already in role '{1}'.", username, roleName));
                    }

                    var bsonDocument = new BsonDocument
                    {
                        { "ApplicationName", this.ApplicationName },
                        { "Role", roleName },
                        { "Username", username }
                    };

                    this._usersInRolesMongoCollection.Insert(bsonDocument);
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));

            if (this._rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
            {
                throw new ProviderException(string.Format("The role '{0}' already exists.", roleName));
            }

            var bsonDocument = new BsonDocument
            {
                { "ApplicationName", this.ApplicationName },
                { "Role", roleName }
            };

            this._rolesMongoCollection.Insert(bsonDocument);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!this.RoleExists(roleName))
            {
                throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
            }

            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));

            if (throwOnPopulatedRole && this._usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
            {
                throw new ProviderException("This role cannot be deleted because there are users present in it.");
            }

            this._usersInRolesMongoCollection.Remove(query);
            this._rolesMongoCollection.Remove(query);

            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (!this.RoleExists(roleName))
            {
                throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
            }

            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this._usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
        }

        public override string[] GetAllRoles()
        {
            var query = Query.EQ("ApplicationName", this.ApplicationName);
            return this._rolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", username));
            return this._usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this._usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;

            var mongoDatabase = new MongoClient(config["connectionString"] ?? "mongodb://localhost").GetServer().GetDatabase(config["database"] ?? "ASPNETDB");
            this._rolesMongoCollection = mongoDatabase.GetCollection(config["collection"] ?? "Roles");
            this._usersInRolesMongoCollection = mongoDatabase.GetCollection("UsersInRoles");

            this._rolesMongoCollection.EnsureIndex("ApplicationName");
            this._rolesMongoCollection.EnsureIndex("ApplicationName", "Role");
            this._usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role");
            this._usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Username");
            this._usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role", "Username");

            base.Initialize(name, config);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
            return this._usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
                    this._usersInRolesMongoCollection.Remove(query);
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
            return this._rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
        }
    }
}
