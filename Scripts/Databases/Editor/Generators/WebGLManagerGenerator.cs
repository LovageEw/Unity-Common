namespace Databases.AutoGenerates
{
    public class WebGLManagerGenerator : ManagerGenerator
    {
        public WebGLManagerGenerator(string outputFolderPath, string outputFilePath, string databaseName)
            : base(outputFolderPath, outputFilePath, databaseName)
        {
        }

        protected override string FileName => "WebGLManagerTemplate";
    }
}