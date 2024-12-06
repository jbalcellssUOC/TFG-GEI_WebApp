namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for prompt operations
    /// </summary>
    public interface IPromptService
    {
        /// <summary>
        /// Launches a prompt to OpenAI API
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns>Returns a string with the result of the prompt answer</returns>
        public Task<string> TriggerPromptOpenAI(string prompt);
    }
}
