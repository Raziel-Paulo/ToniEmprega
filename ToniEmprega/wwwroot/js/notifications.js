/* ============================================
   SISTEMA DE NOTIFICAÇÕES CHAMATIVO - ToniEmprega
   ============================================ */

(function () {
    'use strict';

    // Container de toasts
    let toastContainer = null;

    // ========== INICIALIZAÇÃO ==========
    document.addEventListener('DOMContentLoaded', function () {
        criarToastContainer();
        inicializarDropdownNotificacoes();
        inicializarAlertsInline();
        iniciarPollingNotificacoes();
    });

    // ========== TOAST CONTAINER ==========
    function criarToastContainer() {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container';
        toastContainer.id = 'toastContainer';
        document.body.appendChild(toastContainer);
    }

    // ========== MOSTRAR TOAST ==========
    window.showToast = function (titulo, mensagem, tipo = 'info', duracao = 5000) {
        if (!toastContainer) criarToastContainer();

        const toast = document.createElement('div');
        toast.className = `toast-notification ${tipo}`;

        const icones = {
            success: 'fa-check',
            error: 'fa-times',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };

        const titulosPadrao = {
            success: 'Sucesso!',
            error: 'Erro!',
            warning: 'Atenção!',
            info: 'Informação'
        };

        toast.innerHTML = `
            <div class="toast-icon">
                <i class="fas ${icones[tipo] || icones.info}"></i>
            </div>
            <div class="toast-content">
                <div class="toast-title">${titulo || titulosPadrao[tipo]}</div>
                <div class="toast-message">${mensagem}</div>
            </div>
            <button class="toast-close" onclick="this.closest('.toast-notification').remove()">
                <i class="fas fa-times"></i>
            </button>
        `;

        toastContainer.appendChild(toast);

        // Som opcional (apenas para erros e sucessos importantes)
        if (tipo === 'error' || tipo === 'success') {
            tocarSom(tipo);
        }

        // Auto-remover
        setTimeout(() => {
            toast.classList.add('removing');
            setTimeout(() => toast.remove(), 300);
        }, duracao);

        return toast;
    };

    // ========== SOM DE NOTIFICAÇÃO ==========
    function tocarSom(tipo) {
        // Criar oscillator para som sutil
        try {
            const AudioContext = window.AudioContext || window.webkitAudioContext;
            if (!AudioContext) return;

            const ctx = new AudioContext();
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();

            osc.connect(gain);
            gain.connect(ctx.destination);

            if (tipo === 'success') {
                // Som ascendente positivo
                osc.frequency.setValueAtTime(523, ctx.currentTime);
                osc.frequency.setValueAtTime(659, ctx.currentTime + 0.1);
                osc.frequency.setValueAtTime(784, ctx.currentTime + 0.2);
                gain.gain.setValueAtTime(0.1, ctx.currentTime);
                gain.gain.exponentialRampToValueAtTime(0.01, ctx.currentTime + 0.4);
                osc.start(ctx.currentTime);
                osc.stop(ctx.currentTime + 0.4);
            } else if (tipo === 'error') {
                // Som descendente de erro
                osc.frequency.setValueAtTime(300, ctx.currentTime);
                osc.frequency.setValueAtTime(200, ctx.currentTime + 0.15);
                gain.gain.setValueAtTime(0.1, ctx.currentTime);
                gain.gain.exponentialRampToValueAtTime(0.01, ctx.currentTime + 0.3);
                osc.start(ctx.currentTime);
                osc.stop(ctx.currentTime + 0.3);
            }
        } catch (e) {
            // Silenciosamente falha se audio não suportado
        }
    }

    // ========== DROPDOWN DE NOTIFICAÇÕES ==========
    function inicializarDropdownNotificacoes() {
        const notifToggle = document.getElementById('notifToggle');
        const notifDropdown = document.getElementById('notifDropdown');

        if (!notifToggle || !notifDropdown) return;

        // Toggle ao clicar
        notifToggle.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            notifDropdown.classList.toggle('show');

            if (notifDropdown.classList.contains('show')) {
                carregarNotificacoesDropdown();
            }
        });

        // Fechar ao clicar fora
        document.addEventListener('click', function (e) {
            if (!notifDropdown.contains(e.target) && !notifToggle.contains(e.target)) {
                notifDropdown.classList.remove('show');
            }
        });

        // Marcar todas como lidas
        const markAllBtn = notifDropdown.querySelector('.mark-all-read');
        if (markAllBtn) {
            markAllBtn.addEventListener('click', async function (e) {
                e.preventDefault();
                e.stopPropagation();
                await marcarTodasComoLidas();
            });
        }
    }

    // ========== CARREGAR NOTIFICAÇÕES NO DROPDOWN ==========
    async function carregarNotificacoesDropdown() {
        const listContainer = document.getElementById('notifDropdownList');
        if (!listContainer) return;

        try {
            const response = await fetch('/Notificacoes/ListaRecentes');
            const notificacoes = await response.json();

            if (notificacoes.length === 0) {
                listContainer.innerHTML = `
                    <div class="notif-dropdown-empty">
                        <i class="fas fa-bell-slash"></i>
                        <p>Não tem notificações novas</p>
                    </div>
                `;
                return;
            }

            listContainer.innerHTML = notificacoes.map(n => criarItemDropdown(n)).join('');

            // Adicionar eventos aos itens
            listContainer.querySelectorAll('.notif-dropdown-item').forEach(item => {
                item.addEventListener('click', async function (e) {
                    const notifId = this.dataset.id;
                    const link = this.dataset.link;

                    // Marcar como lida
                    await fetch('/Notificacoes/MarcarComoLida', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                        },
                        body: `id=${notifId}`
                    });

                    // Navegar se tiver link
                    if (link && link !== 'null' && link !== '') {
                        window.location.href = link;
                    } else {
                        this.classList.remove('unread');
                        this.querySelector('.notif-unread-dot')?.remove();
                        atualizarContadorBadge();
                    }
                });
            });

        } catch (error) {
            console.error('Erro ao carregar notificações:', error);
            listContainer.innerHTML = `
                <div class="notif-dropdown-empty">
                    <i class="fas fa-exclamation-circle" style="color: var(--color-danger);"></i>
                    <p>Erro ao carregar notificações</p>
                </div>
            `;
        }
    }

    function criarItemDropdown(n) {
        const icones = {
            success: 'fa-check-circle',
            error: 'fa-times-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };

        const dataFormatada = n.data_Criacao
            ? new Date(n.data_Criacao).toLocaleString('pt-PT', {
                day: '2-digit',
                month: '2-digit',
                hour: '2-digit',
                minute: '2-digit'
            })
            : '';

        return `
            <div class="notif-dropdown-item ${n.lida ? '' : 'unread'}" 
                 data-id="${n.id}" 
                 data-link="${n.link || ''}">
                <div class="notif-item-icon ${n.tipo}">
                    <i class="fas ${icones[n.tipo] || icones.info}"></i>
                </div>
                <div class="notif-item-content">
                    <div class="notif-item-title">
                        ${n.titulo}
                    </div>
                    <div class="notif-item-message">${n.mensagem}</div>
                    <div class="notif-item-time">
                        <i class="far fa-clock"></i> ${dataFormatada}
                    </div>
                </div>
                ${!n.lida ? '<div class="notif-unread-dot"></div>' : ''}
            </div>
        `;
    }

    // ========== MARCAR TODAS COMO LIDAS ==========
    async function marcarTodasComoLidas() {
        try {
            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
            await fetch('/Notificacoes/MarcarTodasComoLidas', {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token
                }
            });

            // Atualizar UI
            document.querySelectorAll('.notif-dropdown-item.unread').forEach(item => {
                item.classList.remove('unread');
                item.querySelector('.notif-unread-dot')?.remove();
            });

            atualizarContadorBadge();
            showToast('Notificações', 'Todas as notificações foram marcadas como lidas.', 'success', 3000);
        } catch (error) {
            console.error('Erro:', error);
        }
    }

    // ========== ATUALIZAR BADGE ==========
    async function atualizarContadorBadge() {
        try {
            const response = await fetch('/Notificacoes/Contador');
            const count = await response.json();

            const badge = document.getElementById('notif-count');
            if (!badge) return;

            if (count > 0) {
                badge.textContent = count > 9 ? '9+' : count;
                badge.style.display = 'flex';
                badge.classList.add('new');
                setTimeout(() => badge.classList.remove('new'), 500);
            } else {
                badge.style.display = 'none';
            }
        } catch (e) {
            console.error('Erro ao atualizar badge:', e);
        }
    }

    // ========== POLLING DE NOTIFICAÇÕES ==========
    let ultimoCount = 0;

    function iniciarPollingNotificacoes() {
        // Primeira verificação
        atualizarContadorBadge();

        // Verificar a cada 15 segundos
        setInterval(async () => {
            try {
                const response = await fetch('/Notificacoes/Contador');
                const count = await response.json();

                if (count > ultimoCount && ultimoCount > 0) {
                    // Nova notificação chegou!
                    const badge = document.getElementById('notif-count');
                    if (badge) {
                        badge.classList.add('new');
                        setTimeout(() => badge.classList.remove('new'), 500);
                    }

                    // Tocar som sutil
                    tocarSom('info');
                }

                ultimoCount = count;
                atualizarContadorBadge();
            } catch (e) {
                // Silenciosamente falha
            }
        }, 15000);
    }

    // ========== ALERTS INLINE MELHORADOS ==========
    function inicializarAlertsInline() {
        // Converter alerts existentes do TempData para toasts
        const tempAlerts = document.querySelectorAll('.main-content > .alert');

        tempAlerts.forEach(alert => {
            let tipo = 'info';
            let titulo = '';

            if (alert.classList.contains('alert-success')) {
                tipo = 'success';
                titulo = 'Sucesso!';
            } else if (alert.classList.contains('alert-error')) {
                tipo = 'error';
                titulo = 'Erro!';
            } else if (alert.classList.contains('alert-warning')) {
                tipo = 'warning';
                titulo = 'Atenção!';
            }

            const mensagem = alert.textContent.trim();

            // Mostrar toast em vez do alert inline
            showToast(titulo, mensagem, tipo, 6000);

            // Esconder o alert original
            alert.style.display = 'none';
        });
    }

    // ========== EXPOR FUNÇÕES GLOBAIS ==========
    window.Notificacoes = {
        showToast: window.showToast,
        atualizarBadge: atualizarContadorBadge,
        recarregarDropdown: carregarNotificacoesDropdown
    };

})();