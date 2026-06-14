using RenData.ChunkIO;
using RenData.IDs;
using RenData.PhysDef;
using RenData.SaveLoad;

namespace RenData.Definitions;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_MOTORCYCLEDEF)]
public partial class MotorcycleDefClass : WheeledVehicleDefClass
{
    public MotorcycleDefClass()
    {
        LeanK0 = 18.0f;
        LeanK1 = 5.0f;
        // make our parameters editable!
        //FLOAT_EDITABLE_PARAM(MotorcycleDefClass, LeanK0, 0.01f, 100000.0f);
        //FLOAT_EDITABLE_PARAM(MotorcycleDefClass, LeanK1, 0.01f, 100000.0f);
    }

    public override uint Get_Class_ID() => ClassId.CLASSID_MOTORCYCLEDEF;
    public override PersistClass Create()
    {
        //MotorcycleClass obj = NEW_REF(MotorcycleClass, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }

    public override string Get_Type_Name() { return "MotorcycleDef"; }
    public override bool Is_Type(string type_name)
    {
        if (string.Compare(type_name, Get_Type_Name()) == 0)
        {
            return true;
        }
        else
        {
            return base.Is_Type(type_name);
        }
    }

    public override PersistFactoryClass Get_Factory() => _persistFactory;
    public override bool Save(ChunkSaveClass csave)
    {
        csave.Begin_Chunk(MOTORCYCLEDEF_CHUNK_WHEELEDVEHICLEDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(MOTORCYCLEDEF_CHUNK_VARIABLES);
        csave.WriteMicro(MOTORCYCLEDEF_VARIABLE_LEANK0, LeanK0);
        csave.WriteMicro(MOTORCYCLEDEF_VARIABLE_LEANK1, LeanK1);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case MOTORCYCLEDEF_CHUNK_WHEELEDVEHICLEDEF:
                    base.Load(cload);
                    break;

                case MOTORCYCLEDEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case MOTORCYCLEDEF_VARIABLE_LEANK0:
                                cload.Read(ref LeanK0);
                                break;
                            case MOTORCYCLEDEF_VARIABLE_LEANK1:
                                cload.Read(ref LeanK1);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //	Editable interface requirements
    //DECLARE_EDITABLE(MotorcycleDefClass, WheeledVehicleDefClass);

    protected float LeanK0;
    protected float LeanK1;

    private const int MOTORCYCLEDEF_CHUNK_WHEELEDVEHICLEDEF = 0x00516000;
    private const int MOTORCYCLEDEF_CHUNK_VARIABLES = 0x00516001;

    private const int MOTORCYCLEDEF_VARIABLE_LEANK0 = 0x00;
    private const int MOTORCYCLEDEF_VARIABLE_LEANK1 = 0x01;
};

