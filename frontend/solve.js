const API_URL = 'http://localhost:5182/api/exams';
const urlParams = new URLSearchParams(window.location.search);
const examId = urlParams.get('examId');

let userAnswers = {}; // przechowuje { questionId: answerId }
let questionsData = [];

const questionsContainer = document.getElementById('questionsContainer');
const checkBtn = document.getElementById('checkBtn');
const actionsFooter = document.getElementById('actionsFooter');
const scoreContainer = document.getElementById('scoreContainer');
const scoreText = document.getElementById('scoreText');

async function init() {
    if (!examId) {
        questionsContainer.innerHTML = `<div class="error">Brak ID egzaminu. Wróć do listy i wybierz maturę.</div>`;
        return;
    }

    try {
        // Fetch exam title
        const examRes = await fetch(`${API_URL}/${examId}`);
        if(examRes.ok) {
            const exam = await examRes.json();
            document.getElementById('examTitle').textContent = exam.title;
        }

        // Fetch questions and answers (without correct/false flags!)
        const res = await fetch(`${API_URL}/${examId}/questions`);
        if (!res.ok) throw new Error('Błąd ładowania pytań');
        
        questionsData = await res.json();
        renderQuestions();
    } catch (err) {
        console.error(err);
        questionsContainer.innerHTML = `<div class="error">Błąd pobierania pytań z API.</div>`;
    }
}

function renderQuestions() {
    if (questionsData.length === 0) {
        questionsContainer.innerHTML = `<div class="loading">Ten egzamin nie ma jeszcze dodanych pytań w bazie.</div>`;
        return;
    }

    let html = '';
    questionsData.forEach((q, index) => {
        html += `
            <div class="question-card" id="q-${q.id}">
                <div class="question-header">
                    <span class="question-number">Zadanie ${q.taskNumber || (index + 1)}</span>
                </div>
                <div class="question-content math-content">${q.content}</div>
                <div class="answers-grid">
                    ${q.answers.map(a => `
                        <button class="answer-btn" id="btn-${a.id}" onclick="selectAnswer(${q.id}, ${a.id})">
                            <span class="math-content">${a.content}</span>
                        </button>
                    `).join('')}
                </div>
            </div>
        `;
    });

    questionsContainer.innerHTML = html;
    actionsFooter.style.display = 'flex';

    // RENDEROWANIE WZORÓW LATEX PRZEZ KATEX
    if (window.renderMathInElement) {
        window.renderMathInElement(questionsContainer, {
            delimiters: [
                {left: '$$', right: '$$', display: true},
                {left: '\\[', right: '\\]', display: true},
                {left: '$', right: '$', display: false},
                {left: '\\(', right: '\\)', display: false}
            ],
            throwOnError: false
        });
    }
}

window.selectAnswer = function(questionId, answerId) {
    userAnswers[questionId] = answerId;
    
    // Reset style for all buttons of this question
    const qData = questionsData.find(q => q.id === questionId);
    qData.answers.forEach(a => {
        const btn = document.getElementById(`btn-${a.id}`);
        if(btn) btn.classList.remove('selected');
    });

    // Mark the selected one
    const selectedBtn = document.getElementById(`btn-${answerId}`);
    if(selectedBtn) selectedBtn.classList.add('selected');
}

checkBtn.addEventListener('click', async () => {
    if (Object.keys(userAnswers).length < questionsData.length) {
        if(!confirm('Zostawiłeś puste odpowiedzi! Czy na pewno chcesz sprawdzić wynik?')) {
            return;
        }
    }

    checkBtn.disabled = true;
    checkBtn.textContent = 'Sprawdzanie na serwerze...';

    try {
        // Send answers to the new anonymous evaluation endpoint
        const res = await fetch(`${API_URL}/${examId}/evaluate`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userAnswers)
        });

        if(!res.ok) throw new Error('Błąd sprawdzania');

        const result = await res.json();
        
        // Zaznaczanie na zielono i czerwono po stronie klienta
        if (result.correctAnswers) {
            // Najpierw usuń stary, niebieski styl ze wszystkich przycisków, by nie nadpisywał kolorów
            document.querySelectorAll('.answer-btn').forEach(btn => btn.classList.remove('selected'));

            Object.keys(result.correctAnswers).forEach(qId => {
                const correctAnsId = result.correctAnswers[qId];
                const userAnsId = userAnswers[qId];

                // Zawsze zaznacz poprawną na zielono
                const correctBtn = document.getElementById(`btn-${correctAnsId}`);
                if (correctBtn) correctBtn.classList.add('correct-answer');

                // Jeśli użytkownik zaznaczył coś innego, zaznacz na czerwono
                if (userAnsId && userAnsId !== correctAnsId) {
                    const wrongBtn = document.getElementById(`btn-${userAnsId}`);
                    if (wrongBtn) wrongBtn.classList.add('wrong-answer');
                }
            });
        } else {
            alert('Wykryto stary backend! Musisz wyłączyć konsolę (CTRL+C) i wpisać dotnet run ponownie, żeby wczytać kod, który wysyła poprawne odpowiedzi do sprawdzenia!');
            document.querySelectorAll('.answer-btn').forEach(btn => btn.classList.remove('selected'));
        }

        // Show score
        scoreContainer.style.display = 'block';
        scoreText.textContent = `Twój wynik: ${result.score} / ${result.totalQuestions || questionsData.length}`;
        
        // Disable answering again
        document.querySelectorAll('.answer-btn').forEach(btn => btn.disabled = true);
        checkBtn.style.display = 'none';
        
        // Auto-scroll to score
        scoreContainer.scrollIntoView({ behavior: 'smooth' });
        
    } catch (err) {
        console.error(err);
        alert('Wystąpił błąd podczas sprawdzania odpowiedzi.');
        checkBtn.disabled = false;
        checkBtn.textContent = 'Sprawdź odpowiedzi!';
    }
});

init();
