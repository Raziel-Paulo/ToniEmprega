/* ============================================
   SISTEMA DE NOTIFICAÇÕES CHAMATIVO - ToniEmprega
   ============================================ */

/* ---------- VARIÁVEIS ---------- */
:root {
    --notif-success: #28a745;
    --notif-error: #dc3545;
    --notif-warning: #ffc107;
    --notif-info: #1E90FF;
    --notif-shadow: 0 8px 32px rgba(0, 0, 0, 0.4);
}

/* ============================================
   TOAST NOTIFICATIONS (flutuantes)
   ============================================ */

.toast-container {
    position: fixed;
    top: 80px;
    right: 20px;
    z-index: 9999;
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
    max-width: 420px;
    pointer-events: none;
}

.toast-notification {
    pointer-events: all;
    background: var(--color-dark-gray);
    border: 1px solid var(--color-navy);
    border-radius: 12px;
    padding: 1rem 1.25rem;
    box-shadow: var(--notif-shadow);
    display: flex;
    align-items: flex-start;
    gap: 0.875rem;
    animation: toastSlideIn 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
    position: relative;
    overflow: hidden;
    backdrop-filter: blur(10px);
}

    .toast-notification.removing {
        animation: toastSlideOut 0.3s ease forwards;
    }

    /* Barra de progresso no topo */
    .toast-notification::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        height: 3px;
        width: 100%;
        animation: toastProgress 5s linear forwards;
    }

    .toast-notification.success::before {
        background: var(--notif-success);
    }

    .toast-notification.error::before {
        background: var(--notif-error);
    }

    .toast-notification.warning::before {
        background: var(--notif-warning);
    }

    .toast-notification.info::before {
        background: var(--notif-info);
    }

/* Ícone animado */
.toast-icon {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.1rem;
    flex-shrink: 0;
    animation: toastIconPop 0.5s ease 0.2s both;
}

.toast-notification.success .toast-icon {
    background: rgba(40, 167, 69, 0.2);
    color: var(--notif-success);
    border: 2px solid rgba(40, 167, 69, 0.3);
}

.toast-notification.error .toast-icon {
    background: rgba(220, 53, 69, 0.2);
    color: var(--notif-error);
    border: 2px solid rgba(220, 53, 69, 0.3);
}

.toast-notification.warning .toast-icon {
    background: rgba(255, 193, 7, 0.2);
    color: var(--notif-warning);
    border: 2px solid rgba(255, 193, 7, 0.3);
}

.toast-notification.info .toast-icon {
    background: rgba(30, 144, 255, 0.2);
    color: var(--notif-info);
    border: 2px solid rgba(30, 144, 255, 0.3);
}

/* Conteúdo */
.toast-content {
    flex: 1;
    min-width: 0;
}

.toast-title {
    color: var(--color-white);
    font-weight: 600;
    font-size: 0.95rem;
    margin-bottom: 0.25rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.toast-message {
    color: var(--color-gray);
    font-size: 0.875rem;
    line-height: 1.5;
}

/* Botão fechar */
.toast-close {
    background: none;
    border: none;
    color: var(--color-gray);
    cursor: pointer;
    padding: 0.25rem;
    font-size: 1rem;
    transition: all 0.2s;
    border-radius: 4px;
    flex-shrink: 0;
}

    .toast-close:hover {
        color: var(--color-white);
        background: rgba(255, 255, 255, 0.1);
    }

/* Glow effect */
.toast-notification.success {
    box-shadow: 0 8px 32px rgba(40, 167, 69, 0.15), var(--notif-shadow);
}

.toast-notification.error {
    box-shadow: 0 8px 32px rgba(220, 53, 69, 0.15), var(--notif-shadow);
}

.toast-notification.warning {
    box-shadow: 0 8px 32px rgba(255, 193, 7, 0.15), var(--notif-shadow);
}

.toast-notification.info {
    box-shadow: 0 8px 32px rgba(30, 144, 255, 0.15), var(--notif-shadow);
}

/* ============================================
   BADGE PULSANTE NO NAVBAR
   ============================================ */

.notif-badge {
    position: absolute;
    top: -6px;
    right: -6px;
    background: var(--color-danger);
    color: white;
    border-radius: 50%;
    min-width: 20px;
    height: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.7rem;
    font-weight: 700;
    padding: 0 4px;
    border: 2px solid var(--color-black);
    animation: badgePulse 2s infinite;
    box-shadow: 0 2px 8px rgba(220, 53, 69, 0.5);
}

    .notif-badge.new {
        animation: badgeBounce 0.5s ease;
    }

/* ============================================
   DROPDOWN DE NOTIFICAÇÕES
   ============================================ */

.notif-dropdown {
    position: relative;
}

.notif-dropdown-menu {
    position: absolute;
    top: calc(100% + 10px);
    right: -10px;
    width: 380px;
    max-height: 500px;
    background: var(--color-dark-gray);
    border: 1px solid var(--color-navy);
    border-radius: 12px;
    box-shadow: 0 12px 40px rgba(0, 0, 0, 0.5);
    overflow: hidden;
    opacity: 0;
    visibility: hidden;
    transform: translateY(-10px) scale(0.95);
    transition: all 0.25s cubic-bezier(0.175, 0.885, 0.32, 1.275);
    z-index: 1000;
}

    .notif-dropdown-menu.show {
        opacity: 1;
        visibility: visible;
        transform: translateY(0) scale(1);
    }

    /* Seta do dropdown */
    .notif-dropdown-menu::before {
        content: '';
        position: absolute;
        top: -6px;
        right: 24px;
        width: 12px;
        height: 12px;
        background: var(--color-dark-gray);
        border-left: 1px solid var(--color-navy);
        border-top: 1px solid var(--color-navy);
        transform: rotate(45deg);
    }

/* Cabeçalho */
.notif-dropdown-header {
    padding: 1rem 1.25rem;
    border-bottom: 1px solid var(--color-navy);
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: var(--color-navy);
}

    .notif-dropdown-header h4 {
        color: var(--color-white);
        margin: 0;
        font-size: 1rem;
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

    .notif-dropdown-header .mark-all-read {
        color: var(--color-blue);
        font-size: 0.8rem;
        cursor: pointer;
        background: none;
        border: none;
        transition: color 0.2s;
    }

        .notif-dropdown-header .mark-all-read:hover {
            color: var(--color-white);
            text-decoration: underline;
        }

/* Lista de notificações */
.notif-dropdown-list {
    max-height: 380px;
    overflow-y: auto;
}

    .notif-dropdown-list::-webkit-scrollbar {
        width: 6px;
    }

    .notif-dropdown-list::-webkit-scrollbar-track {
        background: var(--color-navy);
    }

    .notif-dropdown-list::-webkit-scrollbar-thumb {
        background: var(--color-blue);
        border-radius: 3px;
    }

/* Item individual */
.notif-dropdown-item {
    padding: 1rem 1.25rem;
    border-bottom: 1px solid rgba(11, 37, 69, 0.5);
    display: flex;
    gap: 0.875rem;
    align-items: flex-start;
    cursor: pointer;
    transition: all 0.2s;
    position: relative;
    text-decoration: none;
}

    .notif-dropdown-item:hover {
        background: var(--color-navy);
    }

    .notif-dropdown-item.unread {
        background: rgba(30, 144, 255, 0.05);
    }

        .notif-dropdown-item.unread::before {
            content: '';
            position: absolute;
            left: 0;
            top: 50%;
            transform: translateY(-50%);
            width: 3px;
            height: 60%;
            background: var(--color-blue);
            border-radius: 0 2px 2px 0;
        }

/* Ícone do item */
.notif-item-icon {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.9rem;
    flex-shrink: 0;
}

    .notif-item-icon.success {
        background: rgba(40, 167, 69, 0.15);
        color: var(--notif-success);
    }

    .notif-item-icon.error {
        background: rgba(220, 53, 69, 0.15);
        color: var(--notif-error);
    }

    .notif-item-icon.warning {
        background: rgba(255, 193, 7, 0.15);
        color: var(--notif-warning);
    }

    .notif-item-icon.info {
        background: rgba(30, 144, 255, 0.15);
        color: var(--notif-info);
    }

/* Conteúdo do item */
.notif-item-content {
    flex: 1;
    min-width: 0;
}

.notif-item-title {
    color: var(--color-white);
    font-size: 0.9rem;
    font-weight: 500;
    margin-bottom: 0.2rem;
    display: flex;
    align-items: center;
    gap: 0.4rem;
}

.notif-item-message {
    color: var(--color-gray);
    font-size: 0.8rem;
    line-height: 1.4;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
}

.notif-item-time {
    color: var(--color-gray);
    font-size: 0.75rem;
    margin-top: 0.3rem;
    display: flex;
    align-items: center;
    gap: 0.3rem;
}

/* Ponto de não lido */
.notif-unread-dot {
    width: 8px;
    height: 8px;
    background: var(--color-blue);
    border-radius: 50%;
    flex-shrink: 0;
    margin-top: 0.5rem;
    animation: dotPulse 2s infinite;
}

/* Rodapé */
.notif-dropdown-footer {
    padding: 0.75rem;
    border-top: 1px solid var(--color-navy);
    text-align: center;
    background: var(--color-navy);
}

    .notif-dropdown-footer a {
        color: var(--color-blue);
        font-size: 0.85rem;
        text-decoration: none;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.4rem;
        transition: color 0.2s;
    }

        .notif-dropdown-footer a:hover {
            color: var(--color-white);
        }

/* Estado vazio */
.notif-dropdown-empty {
    padding: 2.5rem 1.5rem;
    text-align: center;
    color: var(--color-gray);
}

    .notif-dropdown-empty i {
        font-size: 2.5rem;
        margin-bottom: 1rem;
        opacity: 0.5;
    }

    .notif-dropdown-empty p {
        margin: 0;
        font-size: 0.9rem;
    }

/* ============================================
   ALERTS INLINE MELHORADOS (TempData)
   ============================================ */

.alert-enhanced {
    border-radius: 10px;
    padding: 1rem 1.25rem;
    margin-bottom: 1.5rem;
    display: flex;
    align-items: flex-start;
    gap: 0.875rem;
    animation: alertSlideDown 0.4s ease;
    position: relative;
    overflow: hidden;
}

    .alert-enhanced::before {
        content: '';
        position: absolute;
        left: 0;
        top: 0;
        bottom: 0;
        width: 4px;
    }

    .alert-enhanced.success {
        background: rgba(40, 167, 69, 0.1);
        border: 1px solid rgba(40, 167, 69, 0.2);
    }

        .alert-enhanced.success::before {
            background: var(--notif-success);
        }

    .alert-enhanced.error {
        background: rgba(220, 53, 69, 0.1);
        border: 1px solid rgba(220, 53, 69, 0.2);
    }

        .alert-enhanced.error::before {
            background: var(--notif-error);
        }

    .alert-enhanced.warning {
        background: rgba(255, 193, 7, 0.1);
        border: 1px solid rgba(255, 193, 7, 0.2);
    }

        .alert-enhanced.warning::before {
            background: var(--notif-warning);
        }

    .alert-enhanced .alert-icon {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.9rem;
        flex-shrink: 0;
    }

    .alert-enhanced.success .alert-icon {
        background: rgba(40, 167, 69, 0.2);
        color: var(--notif-success);
    }

    .alert-enhanced.error .alert-icon {
        background: rgba(220, 53, 69, 0.2);
        color: var(--notif-error);
    }

    .alert-enhanced.warning .alert-icon {
        background: rgba(255, 193, 7, 0.2);
        color: var(--notif-warning);
    }

    .alert-enhanced .alert-content {
        flex: 1;
    }

    .alert-enhanced .alert-title {
        font-weight: 600;
        color: var(--color-white);
        margin-bottom: 0.2rem;
    }

    .alert-enhanced .alert-text {
        color: var(--color-gray);
        font-size: 0.9rem;
    }

    .alert-enhanced .alert-close {
        background: none;
        border: none;
        color: var(--color-gray);
        cursor: pointer;
        padding: 0.2rem;
        font-size: 0.9rem;
        transition: color 0.2s;
        flex-shrink: 0;
    }

        .alert-enhanced .alert-close:hover {
            color: var(--color-white);
        }

/* ============================================
   ANIMAÇÕES KEYFRAMES
   ============================================ */

@keyframes toastSlideIn {
    from {
        opacity: 0;
        transform: translateX(100%) scale(0.9);
    }

    to {
        opacity: 1;
        transform: translateX(0) scale(1);
    }
}

@keyframes toastSlideOut {
    from {
        opacity: 1;
        transform: translateX(0) scale(1);
    }

    to {
        opacity: 0;
        transform: translateX(100%) scale(0.9);
    }
}

@keyframes toastProgress {
    from {
        width: 100%;
    }

    to {
        width: 0%;
    }
}

@keyframes toastIconPop {
    0% {
        transform: scale(0) rotate(-180deg);
    }

    70% {
        transform: scale(1.2) rotate(10deg);
    }

    100% {
        transform: scale(1) rotate(0deg);
    }
}

@keyframes badgePulse {
    0%, 100% {
        transform: scale(1);
        box-shadow: 0 0 0 0 rgba(220, 53, 69, 0.7);
    }

    50% {
        transform: scale(1.05);
        box-shadow: 0 0 0 8px rgba(220, 53, 69, 0);
    }
}

@keyframes badgeBounce {
    0%, 100% {
        transform: scale(1);
    }

    30% {
        transform: scale(1.3);
    }

    50% {
        transform: scale(0.9);
    }

    70% {
        transform: scale(1.1);
    }
}

@keyframes dotPulse {
    0%, 100% {
        opacity: 1;
        transform: scale(1);
    }

    50% {
        opacity: 0.6;
        transform: scale(0.8);
    }
}

@keyframes alertSlideDown {
    from {
        opacity: 0;
        transform: translateY(-20px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

@keyframes shake {
    0%, 100% {
        transform: translateX(0);
    }

    10%, 30%, 50%, 70%, 90% {
        transform: translateX(-3px);
    }

    20%, 40%, 60%, 80% {
        transform: translateX(3px);
    }
}

/* Shake para erros */
.toast-notification.error {
    animation: toastSlideIn 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275), shake 0.5s ease 0.4s;
}

/* ============================================
   RESPONSIVIDADE
   ============================================ */

@media (max-width: 576px) {
    .toast-container {
        left: 10px;
        right: 10px;
        max-width: none;
        top: 70px;
    }

    .notif-dropdown-menu {
        width: calc(100vw - 20px);
        right: -60px;
    }

        .notif-dropdown-menu::before {
            right: 74px;
        }
}
