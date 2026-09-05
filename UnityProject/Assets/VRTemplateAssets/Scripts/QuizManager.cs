using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class JobScores
    {
        public float doctor, mechanic, firefighter, softwareDeveloper, lawyer, teacher;
        public void Add(JobScores other)
        {
            doctor += other.doctor;
            mechanic += other.mechanic;
            firefighter += other.firefighter;
            softwareDeveloper += other.softwareDeveloper;
            lawyer += other.lawyer;
            teacher += other.teacher;
        }
    }

    [System.Serializable]
    public class AnswerOption
    {
        public string text;
        public JobScores scores;
    }

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public List<AnswerOption> answers;
    }

    [Header("UI Elements")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerTexts;
    public GameObject[] answerButtons;
    public GameObject buttonContinue;
    public TextMeshProUGUI progressText;

    private List<Question> questions = new List<Question>();
    private int currentQuestion = 0;
    private JobScores totalScores = new JobScores();
    private string bestJobSceneName;
    private bool isWaiting = false;

    void Awake()
    {
        questions = CareerQuizData.FullQuiz();  // ✅ Correct way to load
    }

    void Start()
    {
        ShowQuestion();
    }

    public void Answer(int index)
    {
        if (isWaiting) return;

        totalScores.Add(questions[currentQuestion].answers[index].scores);

        foreach (var btn in answerButtons)
            btn.SetActive(false);

        StartCoroutine(NextQuestionDelay());
    }

    IEnumerator NextQuestionDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.05f);

        currentQuestion++;

        if (currentQuestion < questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            ShowFinalResult();
        }

        isWaiting = false;
    }

    void ShowQuestion()
    {
        var q = questions[currentQuestion];
        questionText.text = "Q" + (currentQuestion + 1) + ": " + q.questionText;
        progressText.text = (currentQuestion + 1).ToString("00") + "/30";

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].gameObject.SetActive(true);
            answerTexts[i].text = q.answers[i].text;
            answerButtons[i].SetActive(true);
        }
    }

    void ShowFinalResult()
    {
        foreach (var btn in answerButtons)
            btn.SetActive(false);

        foreach (var txt in answerTexts)
            txt.gameObject.SetActive(false);

        progressText.gameObject.SetActive(false);

        string bestJob = "Undetermined";
        float maxScore = -1f;

        void Check(string job, float score, string sceneName)
        {
            if (score > maxScore)
            {
                maxScore = score;
                bestJob = job;
                bestJobSceneName = sceneName;
            }
        }

        Check("Doctor", totalScores.doctor, "Doctor");
        Check("Mechanic", totalScores.mechanic, "Mechanic");
        Check("Firefighter", totalScores.firefighter, "FireFighter");
        Check("Software Developer", totalScores.softwareDeveloper, "SoftwareDeveloper");
        Check("Lawyer", totalScores.lawyer, "Lawyer");
        Check("Teacher", totalScores.teacher, "Teacher");

        questionText.text = "<b>Quiz Complete!</b>\nYour recommended career is:\n<b>" + bestJob + "</b>";
        Debug.Log("Recommended Career: " + bestJob);

        // ✅ Show Continue button
        buttonContinue.SetActive(true);
    }
    public void LoadRecommendedJobScene()
    {
        if (!string.IsNullOrEmpty(bestJobSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(bestJobSceneName);
        }
    }

}

public static class CareerQuizData
{
    public static List<QuizManager.Question> FullQuiz()
    {
        var JS = new System.Func<float, float, float, float, float, float, QuizManager.JobScores>((d, m, f, s, l, t) =>
            new QuizManager.JobScores { doctor = d, mechanic = m, firefighter = f, softwareDeveloper = s, lawyer = l, teacher = t });

        return new List<QuizManager.Question>
        {
            new QuizManager.Question {
                questionText = "Your friend is really upset after failing a test. What would you most likely do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Help them understand what went wrong and offer to study with them.", scores = JS(8, 2, 4, 3, 3, 10) },
                    new QuizManager.AnswerOption { text = "B) Suggest they take a break, go for a walk, and do something practical to feel better.", scores = JS(4, 9, 6, 2, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Stay calm, make sure they’re okay, and offer to talk if they want.", scores = JS(10, 3, 8, 1, 2, 7) },
                    new QuizManager.AnswerOption { text = "D) Look up resources or revision tools and send them something that might help.", scores = JS(6, 4, 2, 10, 3, 5) }
                }
            },
            new QuizManager.Question {
                questionText = "You’ve been put in charge of organising a group activity at school. What’s your first move?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Make sure everyone feels included and knows what to do.", scores = JS(6, 2, 4, 2, 3, 10) },
                    new QuizManager.AnswerOption { text = "B) Plan a safe setup and make sure any risks are covered.", scores = JS(5, 3, 10, 1, 2, 6) },
                    new QuizManager.AnswerOption { text = "C) Build or fix anything you’ll need for the activity.", scores = JS(2, 10, 6, 5, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) Create a shared online plan so people can see roles and updates.", scores = JS(3, 4, 1, 10, 5, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re watching a movie where someone breaks the rules for a good reason. What are you thinking?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Rules are rules, there are better ways to fix things.", scores = JS(5, 2, 4, 3, 10, 6) },
                    new QuizManager.AnswerOption { text = "B) If someone’s in danger, doing the right thing matters more than following rules.", scores = JS(8, 3, 10, 1, 2, 5) },
                    new QuizManager.AnswerOption { text = "C) I’d want to understand both sides before judging.", scores = JS(6, 1, 3, 2, 9, 8) },
                    new QuizManager.AnswerOption { text = "D) I'd redesign the system so people wouldn’t need to break the rules in the first place.", scores = JS(3, 4, 2, 10, 5, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re stuck on a difficult problem. What do you do first?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Break it into smaller parts and look for patterns.", scores = JS(5, 3, 2, 10, 6, 6) },
                    new QuizManager.AnswerOption { text = "B) Ask someone who’s good at it to explain how they’d do it.", scores = JS(7, 2, 3, 3, 4, 10) },
                    new QuizManager.AnswerOption { text = "C) Keep trying until you figure it out hands-on.", scores = JS(3, 10, 5, 5, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) Pause and take a walk to clear your mind before coming back.", scores = JS(6, 4, 6, 2, 3, 5) }
                }
            },
            new QuizManager.Question {
                questionText = "You're asked to help plan a school fundraiser. What excites you most about it?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Making sure the event runs safely and everyone is protected.", scores = JS(7, 3, 10, 2, 3, 5) },
                    new QuizManager.AnswerOption { text = "B) Fixing up equipment or setting up the space for the event.", scores = JS(2, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Writing a speech to inspire others and explain why it matters.", scores = JS(6, 1, 3, 2, 8, 10) },
                    new QuizManager.AnswerOption { text = "D) Designing an app or tool to track donations in real time.", scores = JS(3, 4, 2, 10, 4, 5) }
                }
            },
            new QuizManager.Question {
                questionText = "You notice someone being left out of a group activity. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Invite them over and make sure they feel involved.", scores = JS(7, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Watch to see what’s going on, then step in if needed.", scores = JS(6, 3, 5, 1, 6, 7) },
                    new QuizManager.AnswerOption { text = "C) Focus on getting the activity done, maybe they prefer it that way.", scores = JS(2, 8, 6, 5, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) Suggest a way to change the activity so everyone feels more welcome.", scores = JS(8, 2, 4, 3, 5, 9) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re given a challenging new task that you’ve never done before. What’s your first reaction?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) I feel excited. I like learning and trying new things.", scores = JS(6, 4, 5, 6, 5, 10) },
                    new QuizManager.AnswerOption { text = "B) I want to research everything about it before starting.", scores = JS(7, 2, 3, 10, 6, 5) },
                    new QuizManager.AnswerOption { text = "C) I just start doing it, I’ll figure it out as I go.", scores = JS(3, 10, 6, 4, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) I ask for help from someone experienced and work alongside them.", scores = JS(8, 3, 4, 2, 4, 7) }
                }
            },
            new QuizManager.Question {
                questionText = "Someone in your class gets injured during a PE lesson. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Stay calm, check if they’re okay, and help however you can.", scores = JS(10, 2, 6, 1, 3, 7) },
                    new QuizManager.AnswerOption { text = "B) Move others away, call for help, and make space around them.", scores = JS(7, 4, 10, 1, 2, 5) },
                    new QuizManager.AnswerOption { text = "C) Step back and let staff handle it, you don’t want to make things worse.", scores = JS(3, 5, 2, 1, 4, 3) },
                    new QuizManager.AnswerOption { text = "D) Quickly take notes about what happened to report it clearly.", scores = JS(5, 3, 4, 4, 10, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "Your team disagrees on how to approach a group project. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Listen to everyone's ideas and help the group reach a fair decision.", scores = JS(7, 2, 4, 3, 6, 10) },
                    new QuizManager.AnswerOption { text = "B) Suggest voting on the best idea to keep things moving.", scores = JS(5, 3, 6, 2, 4, 7) },
                    new QuizManager.AnswerOption { text = "C) Take charge and explain why your plan will work best.", scores = JS(4, 5, 7, 3, 10, 4) },
                    new QuizManager.AnswerOption { text = "D) Stay out of it and just focus on doing your part well.", scores = JS(3, 10, 2, 6, 2, 1) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re learning something completely new, and it’s confusing at first. What do you think?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "“A) This is tough, but I’ll keep going until I get it.”", scores = JS(7, 5, 6, 4, 3, 8) },
                    new QuizManager.AnswerOption { text = "“B) I need to understand why it works before I can move on.”", scores = JS(6, 3, 2, 10, 6, 5) },
                    new QuizManager.AnswerOption { text = "“C) I’ll ask someone who’s good at this to walk me through it.”", scores = JS(8, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "“D) Let me take it apart and see how it fits together.”", scores = JS(3, 10, 5, 6, 1, 2) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re watching a documentary about people working under pressure. Which part interests you the most?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) The medical team staying calm and saving lives in emergencies.", scores = JS(10, 2, 6, 1, 4, 5) },
                    new QuizManager.AnswerOption { text = "B) The technicians repairing complex machines during a crisis.", scores = JS(3, 10, 5, 6, 1, 2) },
                    new QuizManager.AnswerOption { text = "C) The emergency response team coordinating rescues.", scores = JS(6, 4, 10, 2, 2, 3) },
                    new QuizManager.AnswerOption { text = "D) The people behind the scenes writing the laws and policies.", scores = JS(4, 1, 2, 3, 10, 6) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re given the chance to run a school club. What kind of club would you start?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) A mental health and support group for students.", scores = JS(10, 2, 3, 1, 4, 9) },
                    new QuizManager.AnswerOption { text = "B) A hands-on building and repair club.", scores = JS(2, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) A safety and emergency preparedness club.", scores = JS(6, 3, 10, 2, 2, 4) },
                    new QuizManager.AnswerOption { text = "D) A coding and innovation club for making apps or games.", scores = JS(3, 4, 2, 10, 5, 5) }
                }
            },
            new QuizManager.Question {
                questionText = "You notice that your school’s recycling system isn’t working well. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Suggest a new system and speak to staff about fixing it.", scores = JS(6, 3, 4, 2, 10, 7) },
                    new QuizManager.AnswerOption { text = "B) Gather some friends to help sort and clean up the bins.", scores = JS(5, 4, 6, 1, 2, 10) },
                    new QuizManager.AnswerOption { text = "C) Build a simple sensor or app to track recycling levels.", scores = JS(3, 6, 2, 10, 4, 4) },
                    new QuizManager.AnswerOption { text = "D) Make a poster or guide to help students recycle better.", scores = JS(7, 2, 3, 3, 5, 9) }
                }
            },
            new QuizManager.Question {
                questionText = "A classmate keeps getting distracted and falling behind. What would you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Offer to help them review and understand the topic.", scores = JS(8, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Suggest a tool or method you’ve used that might help.", scores = JS(6, 3, 2, 10, 5, 6) },
                    new QuizManager.AnswerOption { text = "C) Talk to a teacher privately and share your concern.", scores = JS(7, 1, 4, 1, 6, 9) },
                    new QuizManager.AnswerOption { text = "D) Focus on your own work, it’s not really your problem.", scores = JS(2, 10, 5, 4, 1, 1) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re playing a competitive team game and someone starts arguing unfairly. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Step in and calmly explain the rules to keep things fair.", scores = JS(6, 2, 3, 2, 10, 8) },
                    new QuizManager.AnswerOption { text = "B) Try to calm everyone down and get the game back on track.", scores = JS(8, 3, 6, 1, 4, 10) },
                    new QuizManager.AnswerOption { text = "C) Focus on your role and avoid getting involved.", scores = JS(3, 10, 4, 5, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) Suggest changing the rules slightly so everyone can agree.", scores = JS(5, 4, 2, 6, 6, 7) }
                }
            },
            new QuizManager.Question {
                questionText = "Your class is starting a big project, and you’re given freedom to choose your role. What would you most want to do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Make sure everyone understands the goal and stays on track.", scores = JS(6, 2, 3, 2, 5, 10) },
                    new QuizManager.AnswerOption { text = "B) Take charge of designing and coding the tech side.", scores = JS(3, 4, 2, 10, 5, 4) },
                    new QuizManager.AnswerOption { text = "C) Handle tools, setup, and fixing anything that breaks.", scores = JS(2, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "D) Research the topic and make a strong presentation to the class.", scores = JS(7, 1, 4, 3, 10, 8) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re on a school trip and the schedule suddenly changes. What’s your reaction?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) I stay calm, help others adjust, and try to keep things positive.", scores = JS(8, 2, 5, 1, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) I figure out what needs to happen next and start organising.", scores = JS(6, 3, 6, 2, 5, 9) },
                    new QuizManager.AnswerOption { text = "C) I fix any equipment or setup that’s affected by the change.", scores = JS(2, 10, 4, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "D) I update the plan on my phone and check for solutions online.", scores = JS(3, 4, 2, 10, 6, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re working with someone who keeps making mistakes. What’s your instinct?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Help them figure out what’s going wrong without making them feel bad.", scores = JS(8, 2, 4, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Take over the task to make sure it gets done properly.", scores = JS(3, 10, 6, 6, 1, 2) },
                    new QuizManager.AnswerOption { text = "C) Show them a better way and explain how to do it clearly.", scores = JS(6, 3, 3, 3, 5, 9) },
                    new QuizManager.AnswerOption { text = "D) Let them learn through trial and error unless it becomes risky.", scores = JS(5, 5, 7, 1, 3, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re given an open-ended assignment with almost no instructions. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Ask the teacher questions to understand exactly what’s expected.", scores = JS(6, 3, 4, 2, 8, 9) },
                    new QuizManager.AnswerOption { text = "B) Start outlining your own plan and create a structure from scratch.", scores = JS(5, 4, 3, 6, 6, 8) },
                    new QuizManager.AnswerOption { text = "C) Jump in and figure it out as you go — you prefer working things out hands-on.", scores = JS(3, 10, 5, 5, 1, 3) },
                    new QuizManager.AnswerOption { text = "D) Research how others have done something similar and adapt it.", scores = JS(4, 5, 2, 10, 5, 6) }
                }
            },
            new QuizManager.Question {
                questionText = "You hear a loud crash in the hallway during lunch. What’s your first instinct?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Go check if anyone’s hurt and offer help immediately.", scores = JS(10, 2, 6, 1, 3, 7) },
                    new QuizManager.AnswerOption { text = "B) Move people out of the way and make the area safe.", scores = JS(7, 3, 10, 1, 2, 5) },
                    new QuizManager.AnswerOption { text = "C) Stay where you are and let staff handle it, it’s not your job.", scores = JS(3, 8, 4, 4, 1, 2) },
                    new QuizManager.AnswerOption { text = "D) Record what happened and give a detailed report later.", scores = JS(4, 4, 2, 5, 10, 6) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re working on a team project and someone suggests an idea you don’t agree with. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Ask questions to understand their thinking before responding.", scores = JS(7, 2, 3, 3, 10, 8) },
                    new QuizManager.AnswerOption { text = "B) Suggest a compromise so everyone’s ideas can be included.", scores = JS(6, 3, 4, 2, 6, 10) },
                    new QuizManager.AnswerOption { text = "C) Speak up and explain why your approach is more effective.", scores = JS(4, 5, 6, 4, 9, 5) },
                    new QuizManager.AnswerOption { text = "D) Stick with their idea to avoid any drama, it’s not worth the conflict.", scores = JS(2, 10, 5, 5, 1, 3) }
                }
            },
            new QuizManager.Question {
                questionText = "Your school is running a fundraiser, and there's a problem with tracking the donations. What’s your first move?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Volunteer to sort things out manually and keep records safe.", scores = JS(6, 3, 4, 2, 5, 9) },
                    new QuizManager.AnswerOption { text = "B) Fix the issue using a digital tool or by coding a tracker.", scores = JS(3, 5, 2, 10, 4, 4) },
                    new QuizManager.AnswerOption { text = "C) Speak to a teacher and suggest a system to prevent this in future.", scores = JS(7, 2, 3, 3, 10, 6) },
                    new QuizManager.AnswerOption { text = "D) Take charge and assign roles so everything gets organised quickly.", scores = JS(5, 4, 6, 1, 6, 8) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re helping plan a school event and someone suggests using complicated tech you’ve never tried before. How do you respond?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Offer to learn how it works so you can help.", scores = JS(7, 3, 4, 6, 4, 9) },
                    new QuizManager.AnswerOption { text = "B) Say it’s risky, suggest sticking to what’s proven to work.", scores = JS(6, 4, 6, 2, 7, 5) },
                    new QuizManager.AnswerOption { text = "C) Dive in, experiment, and figure it out as you go.", scores = JS(3, 10, 5, 7, 1, 3) },
                    new QuizManager.AnswerOption { text = "D) Recommend assigning it to someone with experience while you focus elsewhere.", scores = JS(4, 5, 3, 5, 8, 6) }
                }
            },
            new QuizManager.Question {
                questionText = "You’ve finished your work early and notice others are struggling. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Go over and offer to help explain things if they want.", scores = JS(8, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Start tidying or fixing things around the room while you wait.", scores = JS(2, 10, 4, 5, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Check how people are doing and let the teacher know if anyone looks stuck.", scores = JS(7, 3, 5, 1, 6, 8) },
                    new QuizManager.AnswerOption { text = "D) Use the extra time to improve your own work or try something new.", scores = JS(4, 5, 2, 10, 5, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "Your class has to choose a topic for a big project. What kind of topic would you push for?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Something about health, wellbeing, or helping people.", scores = JS(10, 2, 4, 1, 4, 9) },
                    new QuizManager.AnswerOption { text = "B) Something that involves building or designing a real object.", scores = JS(3, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Something about handling emergencies or safety.", scores = JS(6, 3, 10, 2, 2, 5) },
                    new QuizManager.AnswerOption { text = "D) Something involving technology or innovation.", scores = JS(4, 5, 2, 10, 5, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re helping out at a school open day and a guest looks lost. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Approach them kindly, ask if they need help, and guide them to the right place.", scores = JS(8, 2, 3, 1, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Point them in the general direction and get back to your task.", scores = JS(3, 10, 4, 4, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Find a staff member and let them know someone needs help.", scores = JS(6, 3, 5, 2, 5, 8) },
                    new QuizManager.AnswerOption { text = "D) Open the event schedule on your phone and show them the map or info.", scores = JS(4, 5, 2, 10, 6, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "A friend comes to you stressed about exams. What do you do?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Talk it through with them and help them plan a study routine.", scores = JS(7, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Share tools, websites, or apps you’ve used that could help.", scores = JS(4, 3, 2, 10, 5, 5) },
                    new QuizManager.AnswerOption { text = "C) Recommend they speak to a teacher or adult who can support them.", scores = JS(6, 1, 4, 1, 6, 8) },
                    new QuizManager.AnswerOption { text = "D) Offer to study together and explain anything they’re stuck on.", scores = JS(9, 2, 3, 3, 4, 9) }
                }
            },
            new QuizManager.Question {
                questionText = "Your school is upgrading its facilities and asks for student input. What do you suggest?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) A quiet, calm space for students who feel overwhelmed.", scores = JS(10, 2, 3, 1, 4, 9) },
                    new QuizManager.AnswerOption { text = "B) A hands-on workshop space with real tools and materials.", scores = JS(2, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) More emergency equipment and better safety training.", scores = JS(6, 4, 10, 2, 2, 5) },
                    new QuizManager.AnswerOption { text = "D) Smart classrooms with interactive whiteboards and coding kits.", scores = JS(3, 5, 2, 10, 5, 4) }
                }
            },
            new QuizManager.Question {
                questionText = "You’ve been asked to lead a short session for younger students. What’s your approach?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Focus on making them feel safe, heard, and encouraged.", scores = JS(8, 2, 3, 2, 4, 10) },
                    new QuizManager.AnswerOption { text = "B) Use practical activities or demonstrations to keep them engaged.", scores = JS(3, 10, 5, 5, 1, 4) },
                    new QuizManager.AnswerOption { text = "C) Emphasise rules and explain clearly why they matter.", scores = JS(6, 3, 4, 1, 10, 6) },
                    new QuizManager.AnswerOption { text = "D) Build an interactive slideshow or quiz they can use digitally.", scores = JS(4, 5, 2, 10, 5, 5) }
                }
            },
            new QuizManager.Question {
                questionText = "You’re given full freedom to design your dream school subject. What would it focus on?",
                answers = new List<QuizManager.AnswerOption> {
                    new QuizManager.AnswerOption { text = "A) Health, mental wellbeing, and how to take care of others.", scores = JS(10, 2, 4, 1, 4, 9) },
                    new QuizManager.AnswerOption { text = "B) Fixing and building things with real-world applications.", scores = JS(2, 10, 5, 6, 1, 3) },
                    new QuizManager.AnswerOption { text = "C) Emergency situations, teamwork, and staying calm under pressure.", scores = JS(6, 3, 10, 2, 2, 5) },
                    new QuizManager.AnswerOption { text = "D) Creative problem-solving with tech and innovation challenges.", scores = JS(3, 4, 2, 10, 5, 4) }
                }
            },
        };
    }
}
