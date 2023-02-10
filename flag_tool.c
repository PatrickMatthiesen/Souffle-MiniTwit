#include <sqlite3.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* flag is here some kind of tag used on tweets */
char *docStr = "ITU-Minitwit Tweet Flagging Tool\n\n"
               "Usage:\n"
               "  flag_tool <tweet_id>...\n"
               "  flag_tool -i\n"
               "  flag_tool -h\n"
               "Options:\n"
               "-h            Show this screen.\n"
               "-i            Dump all tweets and authors to STDOUT.\n";

static int callback(void *data, int argc, char **argv, char **azColName) { 
  /* where output gets sent */
  printf("%s,%s,%s,%s\n", argv[0] ? argv[0] : "NULL",
         argv[1] ? argv[1] : "NULL", argv[2] ? argv[2] : "NULL",
         argv[4] ? argv[4] : "NULL");
  return 0;
}

int main(int argc, char *argv[]) {
  sqlite3 *db;
  char *zErrMsg = 0;
  int rc; /*remote connection ? */
  char query[256]; /*the tweet - since tweet can be 256 chars long*/
  const char *data = "Callback function called";

  rc = sqlite3_open("/tmp/minitwit.db", &db); /*if rc isn't null an error happend by connecting to database*/
  if (rc) {
    fprintf(stderr, "Can't open database: %s\n", sqlite3_errmsg(db));
    return (0);
  }
  if (argc == 2 && strcmp(argv[1], "-h") == 0) { /*checks if input is = h*/
    fprintf(stdout, "%s\n", docStr); /*-h prints docStr from line 6*/
  }
  if (argc == 2 && strcmp(argv[1], "-i") == 0) { /*checks if input is = i*/
    strcpy(query, "SELECT * FROM message");
    /* Execute SQL statement */
    rc = sqlite3_exec(db, query, callback, (void *)data, &zErrMsg);
    if (rc != SQLITE_OK) { /*if something is wrong prints errorMsg*/
      fprintf(stderr, "SQL error: %s\n", zErrMsg);
      sqlite3_free(zErrMsg);
    }
  }
  if (argc >= 2 && strcmp(argv[1], "-i") != 0 && strcmp(argv[1], "-h") != 0) {
    /* checks if input isn't -h || -i*/
    int i;
    for (i = 1; i < argc; i++) {
      strcpy(query, "UPDATE message SET flagged=1 WHERE message_id=");/*message is here a table*/
      strcat(query, argv[i]); /*updates string query, that gets run later, and concatenets with argv[i] */
      rc = sqlite3_exec(db, query, callback, (void *)data, &zErrMsg);
      if (rc != SQLITE_OK) {/* error handling */
        fprintf(stderr, "SQL error: %s\n", zErrMsg);
        sqlite3_free(zErrMsg); 
      } else {
        printf("Flagged entry: %s\n", argv[i]);
      }
    }
  }

  sqlite3_close(db); /* cloes connection after building executeable flag_tool */
  return 0;
}