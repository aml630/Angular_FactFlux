using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FactFluxV3.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HangFire");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentId = table.Column<int>(nullable: false),
                    ImageLocation = table.Column<string>(maxLength: 150, nullable: true),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "RSSFeeds",
                columns: table => new
                {
                    FeedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FeedTitle = table.Column<string>(maxLength: 250, nullable: true),
                    FeedLink = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    FeedImage = table.Column<string>(unicode: false, nullable: true),
                    VideoLink = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSSFeeds", x => x.FeedId);
                });

            migrationBuilder.CreateTable(
                name: "TwitterUsers",
                columns: table => new
                {
                    TwitterUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TwitterUsername = table.Column<string>(maxLength: 70, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Image = table.Column<string>(maxLength: 90, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterUsers", x => x.TwitterUserId);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Word = table.Column<string>(maxLength: 500, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    Banned = table.Column<bool>(nullable: false),
                    DateIncremented = table.Column<DateTime>(type: "datetime", nullable: true),
                    Daily = table.Column<int>(nullable: false),
                    Monthly = table.Column<int>(nullable: false),
                    Yearly = table.Column<int>(nullable: false),
                    Main = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                });

            migrationBuilder.CreateTable(
                name: "AggregatedCounter",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<long>(nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregatedCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Counter",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<short>(nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hash",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Field = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ExpireAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hash", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<int>(nullable: true),
                    StateName = table.Column<string>(maxLength: 20, nullable: true),
                    InvocationData = table.Column<string>(nullable: false),
                    Arguments = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobQueue",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<int>(nullable: false),
                    Queue = table.Column<string>(maxLength: 50, nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "List",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schema",
                schema: "HangFire",
                columns: table => new
                {
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schema", x => x.Version);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    Data = table.Column<string>(nullable: true),
                    LastHeartbeat = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Set",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleTitle = table.Column<string>(maxLength: 500, nullable: false),
                    ArticleUrl = table.Column<string>(maxLength: 300, nullable: false),
                    ArticleType = table.Column<int>(nullable: false),
                    FeedId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DatePublished = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK__Article__FeedId__25869641",
                        column: x => x.FeedId,
                        principalTable: "RSSFeeds",
                        principalColumn: "FeedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tweets",
                columns: table => new
                {
                    TweetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TwitterUserId = table.Column<int>(nullable: false),
                    EmbedHtml = table.Column<string>(nullable: false),
                    TweetText = table.Column<string>(nullable: false),
                    DateTweeted = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweets", x => x.TweetId);
                    table.ForeignKey(
                        name: "FK__Tweets__TwitterU__7B5B524B",
                        column: x => x.TwitterUserId,
                        principalTable: "TwitterUsers",
                        principalColumn: "TwitterUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParentWords",
                columns: table => new
                {
                    WordJoinId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentWordId = table.Column<int>(nullable: false),
                    ChildWordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentWords", x => x.WordJoinId);
                    table.ForeignKey(
                        name: "FK__ParentWor__Child__35BCFE0A",
                        column: x => x.ChildWordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ParentWor__Paren__36B12243",
                        column: x => x.ParentWordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobParameter",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangFire_JobParameter_Job",
                        column: x => x.JobId,
                        principalSchema: "HangFire",
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "HangFire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Reason = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangFire_State_Job",
                        column: x => x.JobId,
                        principalSchema: "HangFire",
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordLogs",
                columns: table => new
                {
                    WordLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WordId = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime", nullable: false),
                    ArticleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLogs", x => x.WordLogId);
                    table.ForeignKey(
                        name: "FK__WordLogs__Articl__2B3F6F97",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__WordLogs__WordId__2C3393D0",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_FeedId",
                table: "Article",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentWords_ChildWordId",
                table: "ParentWords",
                column: "ChildWordId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentWords_ParentWordId",
                table: "ParentWords",
                column: "ParentWordId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweets_TwitterUserId",
                table: "Tweets",
                column: "TwitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordLogs_ArticleId",
                table: "WordLogs",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_WordLogs_WordId",
                table: "WordLogs",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "UX_HangFire_CounterAggregated_Key",
                schema: "HangFire",
                table: "AggregatedCounter",
                columns: new[] { "Value", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Counter_Key",
                schema: "HangFire",
                table: "Counter",
                columns: new[] { "Value", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Hash_Key",
                schema: "HangFire",
                table: "Hash",
                columns: new[] { "ExpireAt", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Hash_ExpireAt",
                schema: "HangFire",
                table: "Hash",
                columns: new[] { "Id", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "UX_HangFire_Hash_Key_Field",
                schema: "HangFire",
                table: "Hash",
                columns: new[] { "Key", "Field" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Job_StateName",
                schema: "HangFire",
                table: "Job",
                column: "StateName");

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Job_ExpireAt",
                schema: "HangFire",
                table: "Job",
                columns: new[] { "Id", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_JobParameter_JobIdAndName",
                schema: "HangFire",
                table: "JobParameter",
                columns: new[] { "JobId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_JobQueue_QueueAndFetchedAt",
                schema: "HangFire",
                table: "JobQueue",
                columns: new[] { "Queue", "FetchedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_List_ExpireAt",
                schema: "HangFire",
                table: "List",
                columns: new[] { "Id", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_List_Key",
                schema: "HangFire",
                table: "List",
                columns: new[] { "ExpireAt", "Value", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Set_ExpireAt",
                schema: "HangFire",
                table: "Set",
                columns: new[] { "Id", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "UX_HangFire_Set_KeyAndValue",
                schema: "HangFire",
                table: "Set",
                columns: new[] { "Key", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_Set_Key",
                schema: "HangFire",
                table: "Set",
                columns: new[] { "ExpireAt", "Value", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_HangFire_State_JobId",
                schema: "HangFire",
                table: "State",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ParentWords");

            migrationBuilder.DropTable(
                name: "Tweets");

            migrationBuilder.DropTable(
                name: "WordLogs");

            migrationBuilder.DropTable(
                name: "AggregatedCounter",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "Counter",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "Hash",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "JobParameter",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "JobQueue",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "List",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "Schema",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "Server",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "Set",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "State",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "TwitterUsers");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Job",
                schema: "HangFire");

            migrationBuilder.DropTable(
                name: "RSSFeeds");
        }
    }
}
