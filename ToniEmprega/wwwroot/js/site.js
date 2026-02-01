// Funções mínimas para interações (toasts, apply, save)
function applyOffer(form) {
    const fd = new FormData(form);
    fetch(form.action, { method: 'POST', body: fd, headers: { 'X-Requested-With': 'XMLHttpRequest' } })
        .then(r => r.json())
        .then(j => {
            showToast(j.message || 'Ação concluída');
        }).catch(e => showToast('Erro ao enviar candidatura'));
    return false; // prevenir submit normal
}


function toggleSave(offerId, btn) {
    // Simular toggle
    const pressed = btn.getAttribute('aria-pressed') === 'true';
    btn.setAttribute('aria-pressed', (!pressed).toString());
    showToast(pressed ? 'Removido dos guardados' : 'Guardado com sucesso');
}


function showToast(text) {
    const t = document.createElement('div');
    t.className = 'toast';
    t.textContent = text;
    t.style.position = 'fixed'; t.style.top = '20px'; t.style.right = '20px'; t.style.background = '#1b2430'; t.style.color = '#fff'; t.style.padding = '10px 12px'; t.style.borderRadius = '8px';
    document.body.appendChild(t);
    setTimeout(() => { t.remove(); }, 5000);
}