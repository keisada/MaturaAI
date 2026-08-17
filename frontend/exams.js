const API_URL = 'http://localhost:5182/api/exams';
let allExams = [];

const tableBody = document.getElementById('tableBody');
const searchInput = document.getElementById('searchInput');

// Inicjalizacja
async function loadExams() {
    try {
        const response = await fetch(`${API_URL}?page=1&pageSize=100`);
        if (!response.ok) throw new Error('Błąd połączenia z API');
        
        allExams = await response.json();
        renderTable(allExams);
    } catch (error) {
        console.error(error);
        tableBody.innerHTML = `<tr><td colspan="3" class="error">Nie udało się pobrać danych. Upewnij się, że backend działa.</td></tr>`;
    }
}

// Renderowanie tabeli
function renderTable(exams) {
    if (exams.length === 0) {
        tableBody.innerHTML = `<tr><td colspan="3" class="loading">Brak wyników do wyświetlenia.</td></tr>`;
        return;
    }

    tableBody.innerHTML = exams.map(exam => `
        <tr style="cursor: pointer" onclick="window.location.href='solve.html?examId=${exam.id}'">
            <td><span class="subject-badge">${exam.subject}</span></td>
            <td style="font-weight: 600">${exam.title}</td>
            <td class="exam-date">${exam.examMonth} ${exam.examYear}</td>
        </tr>
    `).join('');
}

// Wyszukiwarka
searchInput.addEventListener('input', (e) => {
    const term = e.target.value.toLowerCase();
    const filtered = allExams.filter(exam => 
        (exam.subject && exam.subject.toLowerCase().includes(term)) || 
        (exam.title && exam.title.toLowerCase().includes(term)) ||
        (exam.examMonth && exam.examMonth.toLowerCase().includes(term))
    );
    renderTable(filtered);
});

// Start
loadExams();
