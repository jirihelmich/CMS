﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="CMS")]
	public partial class ACLDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Insertresource(resource instance);
    partial void Updateresource(resource instance);
    partial void Deleteresource(resource instance);
    partial void Insertrole_resource(role_resource instance);
    partial void Updaterole_resource(role_resource instance);
    partial void Deleterole_resource(role_resource instance);
    partial void Insertrole(role instance);
    partial void Updaterole(role instance);
    partial void Deleterole(role instance);
    #endregion
		
		public ACLDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["CMSConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ACLDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ACLDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ACLDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ACLDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<resource> resources
		{
			get
			{
				return this.GetTable<resource>();
			}
		}
		
		public System.Data.Linq.Table<role_resource> role_resources
		{
			get
			{
				return this.GetTable<role_resource>();
			}
		}
		
		public System.Data.Linq.Table<role> roles
		{
			get
			{
				return this.GetTable<role>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.resources")]
	public partial class resource : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _id;
		
		private string _name;
		
		private string _controller;
		
		private string _action;
		
		private System.DateTime _date;
		
		private EntitySet<role_resource> _role_resources;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(long value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OncontrollerChanging(string value);
    partial void OncontrollerChanged();
    partial void OnactionChanging(string value);
    partial void OnactionChanged();
    partial void OndateChanging(System.DateTime value);
    partial void OndateChanged();
    #endregion
		
		public resource()
		{
			this._role_resources = new EntitySet<role_resource>(new Action<role_resource>(this.attach_role_resources), new Action<role_resource>(this.detach_role_resources));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_controller", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string controller
		{
			get
			{
				return this._controller;
			}
			set
			{
				if ((this._controller != value))
				{
					this.OncontrollerChanging(value);
					this.SendPropertyChanging();
					this._controller = value;
					this.SendPropertyChanged("controller");
					this.OncontrollerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_action", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string action
		{
			get
			{
				return this._action;
			}
			set
			{
				if ((this._action != value))
				{
					this.OnactionChanging(value);
					this.SendPropertyChanging();
					this._action = value;
					this.SendPropertyChanged("action");
					this.OnactionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date", DbType="DateTime NOT NULL")]
		public System.DateTime date
		{
			get
			{
				return this._date;
			}
			set
			{
				if ((this._date != value))
				{
					this.OndateChanging(value);
					this.SendPropertyChanging();
					this._date = value;
					this.SendPropertyChanged("date");
					this.OndateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="resource_role_resource", Storage="_role_resources", ThisKey="id", OtherKey="resourcesid")]
		public EntitySet<role_resource> role_resources
		{
			get
			{
				return this._role_resources;
			}
			set
			{
				this._role_resources.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_role_resources(role_resource entity)
		{
			this.SendPropertyChanging();
			entity.resource = this;
		}
		
		private void detach_role_resources(role_resource entity)
		{
			this.SendPropertyChanging();
			entity.resource = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.roles_resources")]
	public partial class role_resource : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _id;
		
		private long _resourcesid;
		
		private long _rolesid;
		
		private System.DateTime _date;
		
		private EntityRef<resource> _resource;
		
		private EntityRef<role> _role;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(long value);
    partial void OnidChanged();
    partial void OnresourcesidChanging(long value);
    partial void OnresourcesidChanged();
    partial void OnrolesidChanging(long value);
    partial void OnrolesidChanged();
    partial void OndateChanging(System.DateTime value);
    partial void OndateChanged();
    #endregion
		
		public role_resource()
		{
			this._resource = default(EntityRef<resource>);
			this._role = default(EntityRef<role>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_resourcesid", DbType="BigInt NOT NULL")]
		public long resourcesid
		{
			get
			{
				return this._resourcesid;
			}
			set
			{
				if ((this._resourcesid != value))
				{
					if (this._resource.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnresourcesidChanging(value);
					this.SendPropertyChanging();
					this._resourcesid = value;
					this.SendPropertyChanged("resourcesid");
					this.OnresourcesidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_rolesid", DbType="BigInt NOT NULL")]
		public long rolesid
		{
			get
			{
				return this._rolesid;
			}
			set
			{
				if ((this._rolesid != value))
				{
					if (this._role.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnrolesidChanging(value);
					this.SendPropertyChanging();
					this._rolesid = value;
					this.SendPropertyChanged("rolesid");
					this.OnrolesidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date", DbType="DateTime NOT NULL")]
		public System.DateTime date
		{
			get
			{
				return this._date;
			}
			set
			{
				if ((this._date != value))
				{
					this.OndateChanging(value);
					this.SendPropertyChanging();
					this._date = value;
					this.SendPropertyChanged("date");
					this.OndateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="resource_role_resource", Storage="_resource", ThisKey="resourcesid", OtherKey="id", IsForeignKey=true)]
		public resource resource
		{
			get
			{
				return this._resource.Entity;
			}
			set
			{
				resource previousValue = this._resource.Entity;
				if (((previousValue != value) 
							|| (this._resource.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._resource.Entity = null;
						previousValue.role_resources.Remove(this);
					}
					this._resource.Entity = value;
					if ((value != null))
					{
						value.role_resources.Add(this);
						this._resourcesid = value.id;
					}
					else
					{
						this._resourcesid = default(long);
					}
					this.SendPropertyChanged("resource");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="role_role_resource", Storage="_role", ThisKey="rolesid", OtherKey="id", IsForeignKey=true)]
		public role role
		{
			get
			{
				return this._role.Entity;
			}
			set
			{
				role previousValue = this._role.Entity;
				if (((previousValue != value) 
							|| (this._role.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._role.Entity = null;
						previousValue.role_resources.Remove(this);
					}
					this._role.Entity = value;
					if ((value != null))
					{
						value.role_resources.Add(this);
						this._rolesid = value.id;
					}
					else
					{
						this._rolesid = default(long);
					}
					this.SendPropertyChanged("role");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.roles")]
	public partial class role : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _id;
		
		private string _name;
		
		private System.DateTime _date;
		
		private System.Nullable<long> _parentid;
		
		private EntitySet<role_resource> _role_resources;
		
		private EntitySet<role> _roles;
		
		private EntityRef<role> _role1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(long value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OndateChanging(System.DateTime value);
    partial void OndateChanged();
    partial void OnparentidChanging(System.Nullable<long> value);
    partial void OnparentidChanged();
    #endregion
		
		public role()
		{
			this._role_resources = new EntitySet<role_resource>(new Action<role_resource>(this.attach_role_resources), new Action<role_resource>(this.detach_role_resources));
			this._roles = new EntitySet<role>(new Action<role>(this.attach_roles), new Action<role>(this.detach_roles));
			this._role1 = default(EntityRef<role>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date", DbType="DateTime NOT NULL")]
		public System.DateTime date
		{
			get
			{
				return this._date;
			}
			set
			{
				if ((this._date != value))
				{
					this.OndateChanging(value);
					this.SendPropertyChanging();
					this._date = value;
					this.SendPropertyChanged("date");
					this.OndateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_parentid", DbType="BigInt")]
		public System.Nullable<long> parentid
		{
			get
			{
				return this._parentid;
			}
			set
			{
				if ((this._parentid != value))
				{
					if (this._role1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnparentidChanging(value);
					this.SendPropertyChanging();
					this._parentid = value;
					this.SendPropertyChanged("parentid");
					this.OnparentidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="role_role_resource", Storage="_role_resources", ThisKey="id", OtherKey="rolesid")]
		public EntitySet<role_resource> role_resources
		{
			get
			{
				return this._role_resources;
			}
			set
			{
				this._role_resources.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="role_role", Storage="_roles", ThisKey="id", OtherKey="parentid")]
		public EntitySet<role> roles
		{
			get
			{
				return this._roles;
			}
			set
			{
				this._roles.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="role_role", Storage="_role1", ThisKey="parentid", OtherKey="id", IsForeignKey=true)]
		public role role1
		{
			get
			{
				return this._role1.Entity;
			}
			set
			{
				role previousValue = this._role1.Entity;
				if (((previousValue != value) 
							|| (this._role1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._role1.Entity = null;
						previousValue.roles.Remove(this);
					}
					this._role1.Entity = value;
					if ((value != null))
					{
						value.roles.Add(this);
						this._parentid = value.id;
					}
					else
					{
						this._parentid = default(Nullable<long>);
					}
					this.SendPropertyChanged("role1");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_role_resources(role_resource entity)
		{
			this.SendPropertyChanging();
			entity.role = this;
		}
		
		private void detach_role_resources(role_resource entity)
		{
			this.SendPropertyChanging();
			entity.role = null;
		}
		
		private void attach_roles(role entity)
		{
			this.SendPropertyChanging();
			entity.role1 = this;
		}
		
		private void detach_roles(role entity)
		{
			this.SendPropertyChanging();
			entity.role1 = null;
		}
	}
}
#pragma warning restore 1591
