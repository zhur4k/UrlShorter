let urlEntries = [];

async function fetchUrlEntries() {
    try {
        const response = await fetch('/url/getAll');
        if (!response.ok) throw new Error('Network response was not ok');
        urlEntries = await response.json();
        renderTable();
    } catch (error) {
        console.error('Ошибка получения URL-записей:', error);
    }
}

function renderTable() {
    const tableBody = document.getElementById('urlTableBody');
    tableBody.innerHTML = '';

    urlEntries.forEach(entry => {
        const row = document.createElement('tr');

        const createdDate = new Date(entry.createdDate);
        const formattedDate = createdDate.toLocaleString();

        const shortUrl = window.location.origin + "/url/" + entry.shortUrl;

        row.innerHTML = `
                 <td><a href="${entry.longUrl}" target="_blank">${entry.longUrl}</a></td>
                <td><a href="${shortUrl}" target="_blank">${shortUrl}</a></td>
                <td>${formattedDate}</td>
                <td>${entry.clickCount}</td>
                <td>
                    <button class="btn btn-warning" onclick="showEditModal(${entry.id})">Edit</button>
                    <button class="btn btn-danger" onclick="deleteUrl(${entry.id})">Delete</button>
                </td>
            `;
        tableBody.appendChild(row);
    });
}

function showAddModal() {
    $('#shortUrl').val('');
    $('#modalTitle').text('Add URL');
    $('#urlModal').modal('show');
}

function closeModal() {
    $('#urlModal').modal('hide');
    $('#urlId').val(null);
    $('#longUrl').val(null);
}

function showEditModal(id) {
    const entry = urlEntries.find(e => e.id === id);
    $('#urlId').val(entry.id); 
    $('#longUrl').val(entry.longUrl);
    $('#modalTitle').text('Edit URL');
    $('#urlModal').modal('show');
}

async function saveUrl(event) {
    event.preventDefault();
    const id = $('#urlId').val();
    const longUrl = $('#longUrl').val();
    let response;
    try {
        if (id) {

            response = await fetch(`/url/edit`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id, longUrl })
            });
        } else {
            response = await fetch(`/url/create`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ longUrl })
            });
        }

        if (!response.ok) throw new Error('Network response was not ok');

        if (id) {
            const entry = urlEntries.find(e => e.id === parseInt(id));
            entry.longUrl = longUrl;
            $('#urlId').val(null);
            $('#longUrl').val(null);
        } else {
            await fetchUrlEntries();
        }

        $('#urlModal').modal('hide');
        renderTable();
    } catch (error) {
        console.error('Ошибка сохранения URL:', error);
    }
}

async function deleteUrl(id) {
    try {
        const response = await fetch(`/url/delete/${id}`, { method: 'POST' }); // Удаление через POST
        if (!response.ok) throw new Error('Network response was not ok');
        urlEntries = urlEntries.filter(e => e.id !== id); // Удаление записи
        renderTable(); // Обновление таблицы
    } catch (error) {
        console.error('Ошибка удаления URL:', error);
    }
}

document.addEventListener('DOMContentLoaded', fetchUrlEntries);
document.getElementById('urlForm').addEventListener('submit', saveUrl);