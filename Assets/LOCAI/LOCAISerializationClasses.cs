using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOCAI
{
    public class TextPart
    {
        public string text { get; set; }
    }

    public class Content
    {
        public List<TextPart> parts { get; set; }
        public string role { get; set; }
    }

    public class SafetyRating
    {
        public string category { get; set; }
        public string probability { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
        public string finishReason { get; set; }
        public int index { get; set; }
        public List<SafetyRating> safetyRatings { get; set; }
    }

    public class UsageMetadata
    {
        public int promptTokenCount { get; set; }
        public int candidatesTokenCount { get; set; }
        public int totalTokenCount { get; set; }
    }

    public class Response
    {
        public List<Candidate> candidates { get; set; }
        public UsageMetadata usageMetadata { get; set; }
    }
}