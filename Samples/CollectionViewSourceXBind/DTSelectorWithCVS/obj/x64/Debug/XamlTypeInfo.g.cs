﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace DTSelectorWithCVS
{
    public partial class App : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
    private global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider _provider;

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            if(_provider == null)
            {
                _provider = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            if(_provider == null)
            {
                _provider = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByName(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }
}

namespace DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[15];
            _typeNameTable[0] = "DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview";
            _typeNameTable[1] = "Windows.UI.Xaml.Controls.DataTemplateSelector";
            _typeNameTable[2] = "Windows.UI.Xaml.DataTemplate";
            _typeNameTable[3] = "DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader";
            _typeNameTable[4] = "DTSelectorWithCVS.MainPage";
            _typeNameTable[5] = "Windows.UI.Xaml.Controls.Page";
            _typeNameTable[6] = "Windows.UI.Xaml.Controls.UserControl";
            _typeNameTable[7] = "System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.test>";
            _typeNameTable[8] = "System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.test>";
            _typeNameTable[9] = "Object";
            _typeNameTable[10] = "DTSelectorWithCVS.test";
            _typeNameTable[11] = "System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.GroupInfoList>";
            _typeNameTable[12] = "System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.GroupInfoList>";
            _typeNameTable[13] = "DTSelectorWithCVS.GroupInfoList";
            _typeNameTable[14] = "String";

            _typeTable = new global::System.Type[15];
            _typeTable[0] = typeof(global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview);
            _typeTable[1] = typeof(global::Windows.UI.Xaml.Controls.DataTemplateSelector);
            _typeTable[2] = typeof(global::Windows.UI.Xaml.DataTemplate);
            _typeTable[3] = typeof(global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader);
            _typeTable[4] = typeof(global::DTSelectorWithCVS.MainPage);
            _typeTable[5] = typeof(global::Windows.UI.Xaml.Controls.Page);
            _typeTable[6] = typeof(global::Windows.UI.Xaml.Controls.UserControl);
            _typeTable[7] = typeof(global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.test>);
            _typeTable[8] = typeof(global::System.Collections.ObjectModel.Collection<global::DTSelectorWithCVS.test>);
            _typeTable[9] = typeof(global::System.Object);
            _typeTable[10] = typeof(global::DTSelectorWithCVS.test);
            _typeTable[11] = typeof(global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.GroupInfoList>);
            _typeTable[12] = typeof(global::System.Collections.ObjectModel.Collection<global::DTSelectorWithCVS.GroupInfoList>);
            _typeTable[13] = typeof(global::DTSelectorWithCVS.GroupInfoList);
            _typeTable[14] = typeof(global::System.String);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_GroupEmptyOrFullSelectorGridview() { return new global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview(); }
        private object Activate_3_GroupEmptyOrFullSelectorGroupheader() { return new global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader(); }
        private object Activate_4_MainPage() { return new global::DTSelectorWithCVS.MainPage(); }
        private object Activate_7_ObservableCollection() { return new global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.test>(); }
        private object Activate_8_Collection() { return new global::System.Collections.ObjectModel.Collection<global::DTSelectorWithCVS.test>(); }
        private object Activate_10_test() { return new global::DTSelectorWithCVS.test(); }
        private object Activate_11_ObservableCollection() { return new global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.GroupInfoList>(); }
        private object Activate_12_Collection() { return new global::System.Collections.ObjectModel.Collection<global::DTSelectorWithCVS.GroupInfoList>(); }
        private object Activate_13_GroupInfoList() { return new global::DTSelectorWithCVS.GroupInfoList(); }
        private void VectorAdd_7_ObservableCollection(object instance, object item)
        {
            var collection = (global::System.Collections.Generic.ICollection<global::DTSelectorWithCVS.test>)instance;
            var newItem = (global::DTSelectorWithCVS.test)item;
            collection.Add(newItem);
        }
        private void VectorAdd_8_Collection(object instance, object item)
        {
            var collection = (global::System.Collections.Generic.ICollection<global::DTSelectorWithCVS.test>)instance;
            var newItem = (global::DTSelectorWithCVS.test)item;
            collection.Add(newItem);
        }
        private void VectorAdd_11_ObservableCollection(object instance, object item)
        {
            var collection = (global::System.Collections.Generic.ICollection<global::DTSelectorWithCVS.GroupInfoList>)instance;
            var newItem = (global::DTSelectorWithCVS.GroupInfoList)item;
            collection.Add(newItem);
        }
        private void VectorAdd_12_Collection(object instance, object item)
        {
            var collection = (global::System.Collections.Generic.ICollection<global::DTSelectorWithCVS.GroupInfoList>)instance;
            var newItem = (global::DTSelectorWithCVS.GroupInfoList)item;
            collection.Add(newItem);
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.DataTemplateSelector"));
                userType.Activator = Activate_0_GroupEmptyOrFullSelectorGridview;
                userType.AddMemberName("Empty");
                userType.AddMemberName("Full");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 1:   //  Windows.UI.Xaml.Controls.DataTemplateSelector
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  Windows.UI.Xaml.DataTemplate
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 3:   //  DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.DataTemplateSelector"));
                userType.Activator = Activate_3_GroupEmptyOrFullSelectorGroupheader;
                userType.AddMemberName("Empty");
                userType.AddMemberName("Full");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 4:   //  DTSelectorWithCVS.MainPage
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_4_MainPage;
                userType.AddMemberName("groups");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 5:   //  Windows.UI.Xaml.Controls.Page
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 6:   //  Windows.UI.Xaml.Controls.UserControl
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 7:   //  System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.test>
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.test>"));
                userType.CollectionAdd = VectorAdd_7_ObservableCollection;
                userType.SetIsReturnTypeStub();
                xamlType = userType;
                break;

            case 8:   //  System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.test>
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_8_Collection;
                userType.CollectionAdd = VectorAdd_8_Collection;
                xamlType = userType;
                break;

            case 9:   //  Object
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 10:   //  DTSelectorWithCVS.test
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_10_test;
                userType.AddMemberName("groupInfoList");
                userType.AddMemberName("heard");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 11:   //  System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.GroupInfoList>
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.GroupInfoList>"));
                userType.CollectionAdd = VectorAdd_11_ObservableCollection;
                userType.SetIsReturnTypeStub();
                xamlType = userType;
                break;

            case 12:   //  System.Collections.ObjectModel.Collection`1<DTSelectorWithCVS.GroupInfoList>
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_12_Collection;
                userType.CollectionAdd = VectorAdd_12_Collection;
                xamlType = userType;
                break;

            case 13:   //  DTSelectorWithCVS.GroupInfoList
                userType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_13_GroupInfoList;
                userType.AddMemberName("Key");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 14:   //  String
                xamlType = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;
            }
            return xamlType;
        }


        private object get_0_GroupEmptyOrFullSelectorGridview_Empty(object instance)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview)instance;
            return that.Empty;
        }
        private void set_0_GroupEmptyOrFullSelectorGridview_Empty(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview)instance;
            that.Empty = (global::Windows.UI.Xaml.DataTemplate)Value;
        }
        private object get_1_GroupEmptyOrFullSelectorGridview_Full(object instance)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview)instance;
            return that.Full;
        }
        private void set_1_GroupEmptyOrFullSelectorGridview_Full(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview)instance;
            that.Full = (global::Windows.UI.Xaml.DataTemplate)Value;
        }
        private object get_2_GroupEmptyOrFullSelectorGroupheader_Empty(object instance)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader)instance;
            return that.Empty;
        }
        private void set_2_GroupEmptyOrFullSelectorGroupheader_Empty(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader)instance;
            that.Empty = (global::Windows.UI.Xaml.DataTemplate)Value;
        }
        private object get_3_GroupEmptyOrFullSelectorGroupheader_Full(object instance)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader)instance;
            return that.Full;
        }
        private void set_3_GroupEmptyOrFullSelectorGroupheader_Full(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader)instance;
            that.Full = (global::Windows.UI.Xaml.DataTemplate)Value;
        }
        private object get_4_MainPage_groups(object instance)
        {
            var that = (global::DTSelectorWithCVS.MainPage)instance;
            return that.groups;
        }
        private void set_4_MainPage_groups(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.MainPage)instance;
            that.groups = (global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.test>)Value;
        }
        private object get_5_test_groupInfoList(object instance)
        {
            var that = (global::DTSelectorWithCVS.test)instance;
            return that.groupInfoList;
        }
        private void set_5_test_groupInfoList(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.test)instance;
            that.groupInfoList = (global::System.Collections.ObjectModel.ObservableCollection<global::DTSelectorWithCVS.GroupInfoList>)Value;
        }
        private object get_6_GroupInfoList_Key(object instance)
        {
            var that = (global::DTSelectorWithCVS.GroupInfoList)instance;
            return that.Key;
        }
        private void set_6_GroupInfoList_Key(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.GroupInfoList)instance;
            that.Key = (global::System.String)Value;
        }
        private object get_7_test_heard(object instance)
        {
            var that = (global::DTSelectorWithCVS.test)instance;
            return that.heard;
        }
        private void set_7_test_heard(object instance, object Value)
        {
            var that = (global::DTSelectorWithCVS.test)instance;
            that.heard = (global::System.String)Value;
        }

        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember xamlMember = null;
            global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType userType;

            switch (longMemberName)
            {
            case "DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview.Empty":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "Empty", "Windows.UI.Xaml.DataTemplate");
                xamlMember.Getter = get_0_GroupEmptyOrFullSelectorGridview_Empty;
                xamlMember.Setter = set_0_GroupEmptyOrFullSelectorGridview_Empty;
                break;
            case "DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview.Full":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.GroupEmptyOrFullSelectorGridview");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "Full", "Windows.UI.Xaml.DataTemplate");
                xamlMember.Getter = get_1_GroupEmptyOrFullSelectorGridview_Full;
                xamlMember.Setter = set_1_GroupEmptyOrFullSelectorGridview_Full;
                break;
            case "DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader.Empty":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "Empty", "Windows.UI.Xaml.DataTemplate");
                xamlMember.Getter = get_2_GroupEmptyOrFullSelectorGroupheader_Empty;
                xamlMember.Setter = set_2_GroupEmptyOrFullSelectorGroupheader_Empty;
                break;
            case "DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader.Full":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.GroupEmptyOrFullSelectorGroupheader");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "Full", "Windows.UI.Xaml.DataTemplate");
                xamlMember.Getter = get_3_GroupEmptyOrFullSelectorGroupheader_Full;
                xamlMember.Setter = set_3_GroupEmptyOrFullSelectorGroupheader_Full;
                break;
            case "DTSelectorWithCVS.MainPage.groups":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.MainPage");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "groups", "System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.test>");
                xamlMember.Getter = get_4_MainPage_groups;
                xamlMember.Setter = set_4_MainPage_groups;
                break;
            case "DTSelectorWithCVS.test.groupInfoList":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.test");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "groupInfoList", "System.Collections.ObjectModel.ObservableCollection`1<DTSelectorWithCVS.GroupInfoList>");
                xamlMember.Getter = get_5_test_groupInfoList;
                xamlMember.Setter = set_5_test_groupInfoList;
                break;
            case "DTSelectorWithCVS.GroupInfoList.Key":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.GroupInfoList");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "Key", "String");
                xamlMember.Getter = get_6_GroupInfoList_Key;
                xamlMember.Setter = set_6_GroupInfoList_Key;
                break;
            case "DTSelectorWithCVS.test.heard":
                userType = (global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlUserType)GetXamlTypeByName("DTSelectorWithCVS.test");
                xamlMember = new global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlMember(this, "heard", "String");
                xamlMember.Getter = get_7_test_heard;
                xamlMember.Setter = set_7_test_heard;
                break;
            }
            return xamlMember;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlSystemBaseType
    {
        global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::DTSelectorWithCVS.DTSelectorWithCVS_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}

