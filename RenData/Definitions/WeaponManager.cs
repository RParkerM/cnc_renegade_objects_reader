/*
** Weapon Manager
*/

using RenData.Definitions;

class WeaponManager
{

	public static WeaponDefinitionClass Find_Weapon_Definition( string name ){ throw new NotImplementedException(); }
    public static WeaponDefinitionClass Find_Weapon_Definition( int id ){ throw new NotImplementedException(); }
    public static AmmoDefinitionClass Find_Ammo_Definition( string ammo_name ){throw new NotImplementedException();}
    public static AmmoDefinitionClass Find_Ammo_Definition( int id ){throw new NotImplementedException();}

    public static bool Is_Weapon_Help_Disabled() { return IsWeaponHelpDisabled; }
    public static void Set_Weapon_Help_Disabled(bool state) { IsWeaponHelpDisabled = state; }

    private static bool IsWeaponHelpDisabled = false;
};


//const WeaponDefinitionClass* WeaponManager::Find_Weapon_Definition( const char *name )
//{
//	return (const WeaponDefinitionClass *)DefinitionMgrClass::Find_Typed_Definition( name, CLASSID_DEF_WEAPON );
//}

//const WeaponDefinitionClass* WeaponManager::Find_Weapon_Definition( int id )
//{
//	return (const WeaponDefinitionClass *)DefinitionMgrClass::Find_Definition( id );
//}

//const AmmoDefinitionClass* WeaponManager::Find_Ammo_Definition( const char *name )
//{
//	return (const AmmoDefinitionClass *)DefinitionMgrClass::Find_Typed_Definition( name, CLASSID_DEF_AMMO );
//}

//const AmmoDefinitionClass* WeaponManager::Find_Ammo_Definition( int id )
//{
//	return (const AmmoDefinitionClass *)DefinitionMgrClass::Find_Definition( id );
//}
