
function openModal(url) {
    fetch(url)
        .then(response => response.text())
        .then(html => {
            const container = document.getElementById('globalModalContainer');
            container.innerHTML = html;
            const modalElement = container.querySelector('.modal');
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        })
        .catch(err => console.error('Error loading modal:', err));
}

document.addEventListener('click', function (e) {
    const target = e.target.closest('[data-modal-url]');
    if (target) {
        const url = target.getAttribute('data-modal-url');
        openModal(url);
    }
});

window.showToast = function (message, type = "info", duration = 3000) {
    const container = document.getElementById('globalToastContainer');

    const toastEl = document.createElement('div');
    toastEl.className = `toast align-items-center text-bg-${type} border-0 show`;
    toastEl.role = "alert";
    toastEl.ariaLive = "assertive";
    toastEl.ariaAtomic = "true";

    toastEl.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">${message}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;

    container.appendChild(toastEl);

    const toast = new bootstrap.Toast(toastEl, { delay: duration });
    toast.show();

    toastEl.addEventListener('hidden.bs.toast', () => {
        toastEl.remove();
    });
}

document.addEventListener('submit', function (e) {
    const form = e.target.closest('form');
    if (!form) return;

    const modalEl = form.closest('.modal');
    if (!modalEl) return;

    e.preventDefault();

    const formData = new FormData(form);

    fetch(form.action, {
        method: form.method || 'POST',
        body: formData
    })
        .then(r => r.json())
        .then(data => {
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();

            if (window.showToast) {
                window.showToast(data.message, data.type);
            }

            if (!data.success) return;

            if (data.redirectUrl) {
                sessionStorage.setItem("pendingToast", JSON.stringify({
                    message: data.message,
                    type: data.type
                }));

                window.location.href = data.redirectUrl;
                return;
            }

            const refreshUrl = form.dataset.refreshUrl;
            const refreshTarget = form.dataset.refreshTarget;

            if (!refreshUrl || !refreshTarget) return;

            fetch(refreshUrl)
                .then(r => r.text())
                .then(html => {
                    const targetEl = document.querySelector(refreshTarget);
                    if (targetEl) {
                        targetEl.innerHTML = html;
                    }
                })
                .catch(err => console.error('Error refreshing target:', err));
        })
        .catch(err => {
            console.error(err);
            if (window.showToast) {
                window.showToast("Unexpected error!", "danger");
            }
        });
});

document.addEventListener("DOMContentLoaded", function () {
    const pending = sessionStorage.getItem("pendingToast");
    if (!pending) return;

    sessionStorage.removeItem("pendingToast");

    try {
        const toast = JSON.parse(pending);
        if (window.showToast) {
            window.showToast(toast.message, toast.type);
        }
    } catch { }
});