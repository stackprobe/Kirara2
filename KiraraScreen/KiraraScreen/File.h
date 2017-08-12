extern struct _finddata_t lastFindData;

void getFileList(char *dir, autoList<char *> *subDirs, autoList<char *> *files);
void updateFindData(char *path);

// ----

int accessible(char *path);
char *refLocalPath(char *path);
void createDir(char *dir);
void deleteDir(char *dir);

char *getCwd(void);
void changeCwd(char *dir);
void addCwd(char *dir);
void unaddCwd(void);

char *getTempDir(void);
char *getAppTempDir(void);
