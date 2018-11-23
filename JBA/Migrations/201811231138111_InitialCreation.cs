namespace JBA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataCollections",
                c => new
                    {
                        DataCollectionID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        CollectionFile = c.Binary(),
                    })
                .PrimaryKey(t => t.DataCollectionID);
            
            CreateTable(
                "dbo.GridReferences",
                c => new
                    {
                        GridReferenceID = c.Int(nullable: false, identity: true),
                        XRef = c.Int(nullable: false),
                        YRef = c.Int(nullable: false),
                        DataCollection_DataCollectionID = c.Int(),
                    })
                .PrimaryKey(t => t.GridReferenceID)
                .ForeignKey("dbo.DataCollections", t => t.DataCollection_DataCollectionID)
                .Index(t => t.DataCollection_DataCollectionID);
            
            CreateTable(
                "dbo.GridReferenceDatas",
                c => new
                    {
                        GridReferenceDataID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                        GridReference_GridReferenceID = c.Int(),
                    })
                .PrimaryKey(t => t.GridReferenceDataID)
                .ForeignKey("dbo.GridReferences", t => t.GridReference_GridReferenceID)
                .Index(t => t.GridReference_GridReferenceID);
            
            CreateTable(
                "dbo.FileHeaders",
                c => new
                    {
                        HeaderID = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        HeaderRegex = c.String(),
                    })
                .PrimaryKey(t => t.HeaderID);
            
            CreateTable(
                "dbo.SimpleDatas",
                c => new
                    {
                        DataID = c.Int(nullable: false, identity: true),
                        XRef = c.Int(nullable: false),
                        YRef = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DataID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridReferenceDatas", "GridReference_GridReferenceID", "dbo.GridReferences");
            DropForeignKey("dbo.GridReferences", "DataCollection_DataCollectionID", "dbo.DataCollections");
            DropIndex("dbo.GridReferenceDatas", new[] { "GridReference_GridReferenceID" });
            DropIndex("dbo.GridReferences", new[] { "DataCollection_DataCollectionID" });
            DropTable("dbo.SimpleDatas");
            DropTable("dbo.FileHeaders");
            DropTable("dbo.GridReferenceDatas");
            DropTable("dbo.GridReferences");
            DropTable("dbo.DataCollections");
        }
    }
}
