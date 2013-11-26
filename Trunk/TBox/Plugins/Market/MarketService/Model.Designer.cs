﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]
namespace MarketService
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class MarketEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new MarketEntities object using the connection string found in the 'MarketEntities' section of the application configuration file.
        /// </summary>
        public MarketEntities() : base("name=MarketEntities", "MarketEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MarketEntities object.
        /// </summary>
        public MarketEntities(string connectionString) : base(connectionString, "MarketEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MarketEntities object.
        /// </summary>
        public MarketEntities(EntityConnection connection) : base(connection, "MarketEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Author> Authors
        {
            get
            {
                if ((_Authors == null))
                {
                    _Authors = base.CreateObjectSet<Author>("Authors");
                }
                return _Authors;
            }
        }
        private ObjectSet<Author> _Authors;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Bug> Bugs
        {
            get
            {
                if ((_Bugs == null))
                {
                    _Bugs = base.CreateObjectSet<Bug>("Bugs");
                }
                return _Bugs;
            }
        }
        private ObjectSet<Bug> _Bugs;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Plugin> Plugins
        {
            get
            {
                if ((_Plugins == null))
                {
                    _Plugins = base.CreateObjectSet<Plugin>("Plugins");
                }
                return _Plugins;
            }
        }
        private ObjectSet<Plugin> _Plugins;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Type> Types
        {
            get
            {
                if ((_Types == null))
                {
                    _Types = base.CreateObjectSet<Type>("Types");
                }
                return _Types;
            }
        }
        private ObjectSet<Type> _Types;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Dependens> Dependenses
        {
            get
            {
                if ((_Dependenses == null))
                {
                    _Dependenses = base.CreateObjectSet<Dependens>("Dependenses");
                }
                return _Dependenses;
            }
        }
        private ObjectSet<Dependens> _Dependenses;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Authors EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAuthors(Author author)
        {
            base.AddObject("Authors", author);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Bugs EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToBugs(Bug bug)
        {
            base.AddObject("Bugs", bug);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Plugins EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPlugins(Plugin plugin)
        {
            base.AddObject("Plugins", plugin);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Types EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToTypes(Type type)
        {
            base.AddObject("Types", type);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Dependenses EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToDependenses(Dependens dependens)
        {
            base.AddObject("Dependenses", dependens);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MarketModel", Name="Author")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Author : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Author object.
        /// </summary>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="uID">Initial value of the UID property.</param>
        public static Author CreateAuthor(global::System.String name, global::System.Int64 uID)
        {
            Author author = new Author();
            author.Name = name;
            author.UID = uID;
            return author;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if (_UID != value)
                {
                    OnUIDChanging(value);
                    ReportPropertyChanging("UID");
                    _UID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UID");
                    OnUIDChanged();
                }
            }
        }
        private global::System.Int64 _UID;
        partial void OnUIDChanging(global::System.Int64 value);
        partial void OnUIDChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MarketModel", Name="Bug")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Bug : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Bug object.
        /// </summary>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="description">Initial value of the Description property.</param>
        /// <param name="date">Initial value of the Date property.</param>
        /// <param name="pluginUID">Initial value of the PluginUID property.</param>
        /// <param name="uID">Initial value of the UID property.</param>
        public static Bug CreateBug(global::System.String name, global::System.String description, global::System.DateTime date, global::System.Int64 pluginUID, global::System.Int64 uID)
        {
            Bug bug = new Bug();
            bug.Name = name;
            bug.Description = description;
            bug.Date = date;
            bug.PluginUID = pluginUID;
            bug.UID = uID;
            return bug;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                OnDescriptionChanging(value);
                ReportPropertyChanging("Description");
                _Description = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Description");
                OnDescriptionChanged();
            }
        }
        private global::System.String _Description;
        partial void OnDescriptionChanging(global::System.String value);
        partial void OnDescriptionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                OnDateChanging(value);
                ReportPropertyChanging("Date");
                _Date = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Date");
                OnDateChanged();
            }
        }
        private global::System.DateTime _Date;
        partial void OnDateChanging(global::System.DateTime value);
        partial void OnDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 PluginUID
        {
            get
            {
                return _PluginUID;
            }
            set
            {
                OnPluginUIDChanging(value);
                ReportPropertyChanging("PluginUID");
                _PluginUID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("PluginUID");
                OnPluginUIDChanged();
            }
        }
        private global::System.Int64 _PluginUID;
        partial void OnPluginUIDChanging(global::System.Int64 value);
        partial void OnPluginUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if (_UID != value)
                {
                    OnUIDChanging(value);
                    ReportPropertyChanging("UID");
                    _UID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UID");
                    OnUIDChanged();
                }
            }
        }
        private global::System.Int64 _UID;
        partial void OnUIDChanging(global::System.Int64 value);
        partial void OnUIDChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MarketModel", Name="Dependens")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Dependens : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Dependens object.
        /// </summary>
        /// <param name="pluginID">Initial value of the PluginID property.</param>
        /// <param name="dependencyID">Initial value of the DependencyID property.</param>
        /// <param name="id">Initial value of the ID property.</param>
        public static Dependens CreateDependens(global::System.Int64 pluginID, global::System.Int64 dependencyID, global::System.Int64 id)
        {
            Dependens dependens = new Dependens();
            dependens.PluginID = pluginID;
            dependens.DependencyID = dependencyID;
            dependens.ID = id;
            return dependens;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 PluginID
        {
            get
            {
                return _PluginID;
            }
            set
            {
                OnPluginIDChanging(value);
                ReportPropertyChanging("PluginID");
                _PluginID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("PluginID");
                OnPluginIDChanged();
            }
        }
        private global::System.Int64 _PluginID;
        partial void OnPluginIDChanging(global::System.Int64 value);
        partial void OnPluginIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 DependencyID
        {
            get
            {
                return _DependencyID;
            }
            set
            {
                OnDependencyIDChanging(value);
                ReportPropertyChanging("DependencyID");
                _DependencyID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DependencyID");
                OnDependencyIDChanged();
            }
        }
        private global::System.Int64 _DependencyID;
        partial void OnDependencyIDChanging(global::System.Int64 value);
        partial void OnDependencyIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int64 _ID;
        partial void OnIDChanging(global::System.Int64 value);
        partial void OnIDChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MarketModel", Name="Plugin")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Plugin : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Plugin object.
        /// </summary>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="date">Initial value of the Date property.</param>
        /// <param name="author">Initial value of the Author property.</param>
        /// <param name="type">Initial value of the Type property.</param>
        /// <param name="description">Initial value of the Description property.</param>
        /// <param name="downloads">Initial value of the Downloads property.</param>
        /// <param name="uploads">Initial value of the Uploads property.</param>
        /// <param name="size">Initial value of the Size property.</param>
        /// <param name="uID">Initial value of the UID property.</param>
        /// <param name="isPlugin">Initial value of the IsPlugin property.</param>
        public static Plugin CreatePlugin(global::System.String name, global::System.DateTime date, global::System.Int64 author, global::System.Int64 type, global::System.String description, global::System.Int64 downloads, global::System.Int64 uploads, global::System.Int64 size, global::System.Int64 uID, global::System.Boolean isPlugin)
        {
            Plugin plugin = new Plugin();
            plugin.Name = name;
            plugin.Date = date;
            plugin.Author = author;
            plugin.Type = type;
            plugin.Description = description;
            plugin.Downloads = downloads;
            plugin.Uploads = uploads;
            plugin.Size = size;
            plugin.UID = uID;
            plugin.IsPlugin = isPlugin;
            return plugin;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                OnDateChanging(value);
                ReportPropertyChanging("Date");
                _Date = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Date");
                OnDateChanged();
            }
        }
        private global::System.DateTime _Date;
        partial void OnDateChanging(global::System.DateTime value);
        partial void OnDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Author
        {
            get
            {
                return _Author;
            }
            set
            {
                OnAuthorChanging(value);
                ReportPropertyChanging("Author");
                _Author = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Author");
                OnAuthorChanged();
            }
        }
        private global::System.Int64 _Author;
        partial void OnAuthorChanging(global::System.Int64 value);
        partial void OnAuthorChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Type
        {
            get
            {
                return _Type;
            }
            set
            {
                OnTypeChanging(value);
                ReportPropertyChanging("Type");
                _Type = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Type");
                OnTypeChanged();
            }
        }
        private global::System.Int64 _Type;
        partial void OnTypeChanging(global::System.Int64 value);
        partial void OnTypeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                OnDescriptionChanging(value);
                ReportPropertyChanging("Description");
                _Description = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Description");
                OnDescriptionChanged();
            }
        }
        private global::System.String _Description;
        partial void OnDescriptionChanging(global::System.String value);
        partial void OnDescriptionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Downloads
        {
            get
            {
                return _Downloads;
            }
            set
            {
                OnDownloadsChanging(value);
                ReportPropertyChanging("Downloads");
                _Downloads = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Downloads");
                OnDownloadsChanged();
            }
        }
        private global::System.Int64 _Downloads;
        partial void OnDownloadsChanging(global::System.Int64 value);
        partial void OnDownloadsChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Uploads
        {
            get
            {
                return _Uploads;
            }
            set
            {
                OnUploadsChanging(value);
                ReportPropertyChanging("Uploads");
                _Uploads = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Uploads");
                OnUploadsChanged();
            }
        }
        private global::System.Int64 _Uploads;
        partial void OnUploadsChanging(global::System.Int64 value);
        partial void OnUploadsChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Size
        {
            get
            {
                return _Size;
            }
            set
            {
                OnSizeChanging(value);
                ReportPropertyChanging("Size");
                _Size = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Size");
                OnSizeChanged();
            }
        }
        private global::System.Int64 _Size;
        partial void OnSizeChanging(global::System.Int64 value);
        partial void OnSizeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if (_UID != value)
                {
                    OnUIDChanging(value);
                    ReportPropertyChanging("UID");
                    _UID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UID");
                    OnUIDChanged();
                }
            }
        }
        private global::System.Int64 _UID;
        partial void OnUIDChanging(global::System.Int64 value);
        partial void OnUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean IsPlugin
        {
            get
            {
                return _IsPlugin;
            }
            set
            {
                OnIsPluginChanging(value);
                ReportPropertyChanging("IsPlugin");
                _IsPlugin = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsPlugin");
                OnIsPluginChanged();
            }
        }
        private global::System.Boolean _IsPlugin;
        partial void OnIsPluginChanging(global::System.Boolean value);
        partial void OnIsPluginChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="MarketModel", Name="Type")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Type : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Type object.
        /// </summary>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="uID">Initial value of the UID property.</param>
        public static Type CreateType(global::System.String name, global::System.Int64 uID)
        {
            Type type = new Type();
            type.Name = name;
            type.UID = uID;
            return type;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if (_UID != value)
                {
                    OnUIDChanging(value);
                    ReportPropertyChanging("UID");
                    _UID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("UID");
                    OnUIDChanged();
                }
            }
        }
        private global::System.Int64 _UID;
        partial void OnUIDChanging(global::System.Int64 value);
        partial void OnUIDChanged();

        #endregion

    
    }

    #endregion

    
}
