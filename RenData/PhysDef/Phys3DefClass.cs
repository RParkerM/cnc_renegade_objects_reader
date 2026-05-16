using RenData.ChunkIO;
using RenData.IDs;
using RenData.PhysDef;
using RenData.SaveLoad;

namespace RenData;

[RegisterDefinition(ChunkId.PHYSICS_CHUNKID_PHYS3DEF)]
public partial class Phys3DefClass : MoveablePhysDefClass
{
    public Phys3DefClass()
    {
        NormSpeed = Phys3.DEFAULT_NORMALIZED_SPEED;
        SlideAngle = Phys3.DEFAULT_SLIDE_ANGLE;
        StepHeight = Phys3.DEFAULT_STEP_HEIGHT;
        // make our parameters editable!
        //EDITABLE_PARAM(Phys3DefClass, ParameterClass::TYPE_FLOAT, NormSpeed);
        //ANGLE_EDITABLE_PARAM(Phys3DefClass, SlideAngle, DEG_TO_RADF(0.0f), DEG_TO_RADF(90.0f));
        //FLOAT_EDITABLE_PARAM(Phys3DefClass, StepHeight, 0.0f, 10.0f);
    }
    public override uint Get_Class_ID() => ClassId.CLASSID_PHYS3DEF;
    public override PersistClass Create()
    {
        //Phys3Class obj = NEW_REF(Phys3Class, ());
        //obj.Init(this);
        //return obj;
        throw new NotImplementedException();
    }
    public override string Get_Type_Name() { return "Phys3Def"; }
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
        csave.Begin_Chunk(PHYS3DEF_CHUNK_MOVEABLEPHYSDEF);
        base.Save(csave);
        csave.End_Chunk();

        csave.Begin_Chunk(PHYS3DEF_CHUNK_VARIABLES);
        csave.WriteMicro(PHYS3DEF_VARIABLE_NORMSPEED, NormSpeed);
        csave.WriteMicro(PHYS3DEF_VARIABLE_SLIDEANGLE, SlideAngle);
        csave.WriteMicro(PHYS3DEF_VARIABLE_STEPHEIGHT, StepHeight);
        csave.End_Chunk();
        return true;
    }
    public override bool Load(ChunkLoadClass cload)
    {
        while (cload.Open_Chunk())
        {

            switch (cload.Cur_Chunk_ID)
            {

                case PHYS3DEF_CHUNK_MOVEABLEPHYSDEF:
                    base.Load(cload);
                    break;

                case PHYS3DEF_CHUNK_VARIABLES:
                    while (cload.Open_Micro_Chunk())
                    {
                        switch (cload.Cur_Micro_Chunk_ID)
                        {
                            case PHYS3DEF_VARIABLE_NORMSPEED:
                                cload.Read(ref NormSpeed);
                                break;
                            case PHYS3DEF_VARIABLE_SLIDEANGLE:
                                cload.Read(ref SlideAngle);
                                break;
                            case PHYS3DEF_VARIABLE_STEPHEIGHT:
                                cload.Read(ref StepHeight);
                                break;
                        }
                        cload.Close_Micro_Chunk();
                    }
                    break;

                default:
                    Console.WriteLine("Unhandled Chunk: {cload.Cur_Chunk_ID}");
                    break;
            }

            cload.Close_Chunk();
        }
        return true;
    }

    //DECLARE_EDITABLE(Phys3DefClass, MoveablePhysDefClass);

    protected float NormSpeed;          // speed to move when controller is 1.0
    protected float SlideAngle;         // slope angle at which this object slides off 
    protected float StepHeight;			// step side that this object will hop over

    private const int PHYS3DEF_CHUNK_MOVEABLEPHYSDEF = 0x04486000;
    private const int PHYS3DEF_CHUNK_VARIABLES = 0x04486001;

    private const int PHYS3DEF_VARIABLE_NORMSPEED = 0x00;
    private const int PHYS3DEF_VARIABLE_SLIDEANGLE = 0x01;
    private const int PHYS3DEF_VARIABLE_STEPHEIGHT = 0x02;
};
