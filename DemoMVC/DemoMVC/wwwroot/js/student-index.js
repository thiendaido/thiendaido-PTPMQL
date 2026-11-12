(() => {
    const listUrl = "/Student/List";
    const detailsUrl = "/Student/Details";
    const createUrl = "/Student/Create";
    const updateUrl = "/Student/Update";
    const deleteUrl = "/Student/Delete";
    const form = document.getElementById("studentForm");
    const modalElement = document.getElementById("studentModal");
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    const rows = document.getElementById("studentRows");
    const pagination = document.getElementById("pagination");
    const paginationInfo = document.getElementById("paginationInfo");
    const searchInput = document.getElementById("studentSearch");
    const pageSizeInput = document.getElementById("pageSize");
    const pageAlert = document.getElementById("pageAlert");
    const formSummary = document.getElementById("formValidationSummary");
    const deleteConfirmation = document.getElementById("deleteConfirmation");
    const saveButton = document.getElementById("saveStudentButton");
    const deleteButton = document.getElementById("deleteStudentButton");
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    const state = { page: 1, pageSize: 10, search: "", totalPages: 1, editingId: null, deletingId: null };
    const fieldNames = ["Id", "HoTen", "NgaySinh", "GioiTinh", "Email", "MaKhoa", "NamNhapHoc", "HeDaoTao"];

    function showPageError(message) {
        pageAlert.textContent = message;
        pageAlert.classList.remove("alert-success");
        pageAlert.classList.add("alert-danger");
        pageAlert.classList.remove("d-none");
    }

    function clearPageError() {
        pageAlert.textContent = "";
        pageAlert.classList.add("d-none");
    }

    function showPageMessage(message, type) {
        pageAlert.textContent = message;
        pageAlert.classList.remove("d-none", "alert-danger", "alert-success");
        pageAlert.classList.add(`alert-${type}`);
        window.setTimeout(() => {
            pageAlert.classList.add("d-none");
            pageAlert.classList.remove("alert-success");
            pageAlert.classList.add("alert-danger");
        }, 3000);
    }

    function setLoading(message) {
        rows.replaceChildren();
        const row = document.createElement("tr");
        const cell = document.createElement("td");
        cell.colSpan = 9;
        cell.className = "text-center text-muted py-4";
        cell.textContent = message;
        row.append(cell);
        rows.append(row);
    }

    async function request(url, options = {}) {
        const response = await fetch(url, {
            ...options,
            headers: {
                Accept: "application/json",
                ...(options.body ? { "Content-Type": "application/json" } : {}),
                ...(options.method && options.method !== "GET" ? { RequestVerificationToken: token } : {}),
                ...options.headers
            }
        });
        const contentType = response.headers.get("content-type") || "";
        const data = contentType.includes("application/json")
            ? await response.json()
            : null;

        if (!response.ok) {
            const error = new Error(data?.message || `Máy chủ trả về lỗi HTTP ${response.status}.`);
            error.data = data;
            error.status = response.status;
            throw error;
        }

        if (data === null) {
            throw new Error("Máy chủ trả về dữ liệu không đúng định dạng.");
        }

        return data;
    }

    function createActionButton(label, action, id, style) {
        const button = document.createElement("button");
        button.type = "button";
        button.className = `btn btn-sm ${style}`;
        button.textContent = label;
        button.dataset.action = action;
        button.dataset.id = id;
        return button;
    }

    function renderStudents(items) {
        rows.replaceChildren();

        if (items.length === 0) {
            setLoading(state.search ? "Không tìm thấy sinh viên phù hợp." : "Chưa có sinh viên nào.");
            return;
        }

        for (const student of items) {
            const row = document.createElement("tr");
            const values = [
                student.id,
                student.hoTen,
                student.ngaySinh ? new Date(student.ngaySinh).toLocaleDateString("vi-VN") : "",
                student.gioiTinh,
                student.email || "",
                student.tenKhoa || "",
                student.namNhapHoc,
                student.heDaoTao
            ];

            for (const value of values) {
                const cell = document.createElement("td");
                cell.textContent = value ?? "";
                row.append(cell);
            }

            const actions = document.createElement("td");
            actions.className = "text-end text-nowrap";
            actions.append(
                createActionButton("Xem", "details", student.id, "btn-outline-secondary me-1"),
                createActionButton("Sửa", "edit", student.id, "btn-outline-primary me-1"),
                createActionButton("Xóa", "delete", student.id, "btn-outline-danger")
            );
            row.append(actions);
            rows.append(row);
        }
    }

    function addPageButton(label, page, disabled, active = false, ariaLabel = label) {
        const item = document.createElement("li");
        item.className = `page-item${disabled ? " disabled" : ""}${active ? " active" : ""}`;
        const button = document.createElement("button");
        button.type = "button";
        button.className = "page-link";
        button.textContent = label;
        button.setAttribute("aria-label", ariaLabel);
        button.disabled = disabled;
        button.dataset.page = String(page);
        item.append(button);
        pagination.append(item);
    }

    function renderPagination(data) {
        pagination.replaceChildren();
        state.totalPages = data.totalPages;

        const firstItem = data.totalItems === 0 ? 0 : (data.page - 1) * data.pageSize + 1;
        const lastItem = Math.min(data.page * data.pageSize, data.totalItems);
        paginationInfo.textContent = `Hiển thị ${firstItem}-${lastItem} trong ${data.totalItems} sinh viên`;

        addPageButton("‹", data.page - 1, data.page <= 1, false, "Trang trước");
        const startPage = Math.max(1, Math.min(data.page - 2, data.totalPages - 4));
        const endPage = Math.min(data.totalPages, startPage + 4);

        for (let page = startPage; page <= endPage; page += 1) {
            addPageButton(String(page), page, false, page === data.page, `Trang ${page}`);
        }

        addPageButton("›", data.page + 1, data.page >= data.totalPages, false, "Trang sau");
    }

    async function loadStudents() {
        clearPageError();
        setLoading("Đang tải dữ liệu...");
        const query = new URLSearchParams({
            search: state.search,
            page: String(state.page),
            pageSize: String(state.pageSize)
        });

        try {
            const data = await request(`${listUrl}?${query}`);
            state.page = data.page;
            renderStudents(data.items);
            renderPagination(data);
        } catch (error) {
            setLoading("Không thể tải danh sách sinh viên.");
            showPageError(error.message);
        }
    }

    function clearValidation() {
        formSummary.textContent = "";
        formSummary.classList.add("d-none");
        for (const name of fieldNames) {
            const field = form.elements.namedItem(name);
            field.classList.remove("is-invalid");
            const message = form.querySelector(`[data-valmsg-for="${name}"]`);
            message.textContent = "";
        }
    }

    function showValidation(data) {
        clearValidation();
        formSummary.textContent = data.message || "Vui lòng kiểm tra lại dữ liệu.";
        formSummary.classList.remove("d-none");

        for (const [name, messages] of Object.entries(data.errors || {})) {
            const fieldName = fieldNames.find(field => field.toLowerCase() === name.split(".").pop().toLowerCase());
            if (!fieldName) {
                continue;
            }

            const field = form.elements.namedItem(fieldName);
            field.classList.add("is-invalid");
            form.querySelector(`[data-valmsg-for="${fieldName}"]`).textContent = messages.join(" ");
        }
    }

    function setFormValues(student) {
        form.elements.namedItem("Id").value = student.id || "";
        form.elements.namedItem("HoTen").value = student.hoTen || "";
        form.elements.namedItem("NgaySinh").value = student.ngaySinh ? student.ngaySinh.slice(0, 10) : "";
        form.elements.namedItem("GioiTinh").value = student.gioiTinh || "";
        form.elements.namedItem("Email").value = student.email || "";
        form.elements.namedItem("MaKhoa").value = student.maKhoa || "";
        form.elements.namedItem("NamNhapHoc").value = student.namNhapHoc || "";
        form.elements.namedItem("HeDaoTao").value = student.heDaoTao || "";
    }

    function setFormReadOnly(readOnly) {
        for (const name of fieldNames) {
            form.elements.namedItem(name).disabled = readOnly;
        }
    }

    function openCreateModal() {
        state.editingId = null;
        state.deletingId = null;
        clearValidation();
        form.reset();
        form.elements.namedItem("NamNhapHoc").value = new Date().getFullYear();
        form.elements.namedItem("HeDaoTao").value = "21";
        setFormReadOnly(false);
        form.elements.namedItem("Id").readOnly = false;
        document.getElementById("studentModalTitle").textContent = "Thêm sinh viên";
        deleteConfirmation.classList.add("d-none");
        form.classList.remove("d-none");
        saveButton.classList.remove("d-none");
        deleteButton.classList.add("d-none");
        modal.show();
    }

    async function openStudentModal(mode, id) {
        clearValidation();
        deleteConfirmation.classList.add("d-none");
        form.classList.remove("d-none");
        saveButton.classList.add("d-none");
        deleteButton.classList.add("d-none");
        setFormReadOnly(true);
        document.getElementById("studentModalTitle").textContent = "Đang tải...";
        modal.show();

        try {
            const student = await request(`${detailsUrl}?id=${encodeURIComponent(id)}`);
            setFormValues(student);

            if (mode === "details") {
                document.getElementById("studentModalTitle").textContent = "Thông tin sinh viên";
            } else if (mode === "edit") {
                state.editingId = student.id;
                setFormReadOnly(false);
                form.elements.namedItem("Id").readOnly = true;
                saveButton.textContent = "Cập nhật";
                saveButton.classList.remove("d-none");
                document.getElementById("studentModalTitle").textContent = "Cập nhật sinh viên";
            } else {
                state.deletingId = student.id;
                form.classList.add("d-none");
                deleteConfirmation.textContent = `Bạn có chắc chắn muốn xóa sinh viên ${student.hoTen} (${student.id})?`;
                deleteConfirmation.classList.remove("d-none");
                deleteButton.classList.remove("d-none");
                document.getElementById("studentModalTitle").textContent = "Xác nhận xóa";
            }
        } catch (error) {
            modal.hide();
            showPageError(error.message);
        }
    }

    function getStudentPayload() {
        return {
            id: form.elements.namedItem("Id").value.trim(),
            hoTen: form.elements.namedItem("HoTen").value.trim(),
            ngaySinh: form.elements.namedItem("NgaySinh").value || null,
            gioiTinh: form.elements.namedItem("GioiTinh").value.trim(),
            email: form.elements.namedItem("Email").value.trim() || null,
            maKhoa: form.elements.namedItem("MaKhoa").value,
            namNhapHoc: Number(form.elements.namedItem("NamNhapHoc").value),
            heDaoTao: form.elements.namedItem("HeDaoTao").value.trim()
        };
    }

    document.getElementById("addStudentButton").addEventListener("click", openCreateModal);

    rows.addEventListener("click", event => {
        const button = event.target.closest("button[data-action]");
        if (!button) {
            return;
        }

        openStudentModal(button.dataset.action, button.dataset.id);
    });

    pagination.addEventListener("click", event => {
        const button = event.target.closest("button[data-page]");
        if (!button || button.disabled) {
            return;
        }

        state.page = Number(button.dataset.page);
        loadStudents();
    });

    searchInput.addEventListener("input", () => {
        window.clearTimeout(searchInput.searchTimer);
        searchInput.searchTimer = window.setTimeout(() => {
            state.search = searchInput.value.trim();
            state.page = 1;
            loadStudents();
        }, 300);
    });

    pageSizeInput.addEventListener("change", () => {
        state.pageSize = Number(pageSizeInput.value);
        state.page = 1;
        loadStudents();
    });

    form.addEventListener("input", event => {
        if (event.target.name) {
            event.target.classList.remove("is-invalid");
            form.querySelector(`[data-valmsg-for="${event.target.name}"]`).textContent = "";
        }
    });

    form.addEventListener("submit", async event => {
        event.preventDefault();
        clearValidation();
        if (!form.reportValidity()) {
            return;
        }

        const isEditing = state.editingId !== null;
        const url = isEditing
            ? `${updateUrl}?id=${encodeURIComponent(state.editingId)}`
            : createUrl;

        try {
            const result = await request(url, {
                method: "POST",
                body: JSON.stringify(getStudentPayload())
            });
            modal.hide();
            await loadStudents();
            showPageMessage(result.message, "success");
        } catch (error) {
            if (error.status === 400 && error.data.errors) {
                showValidation(error.data);
            } else {
                formSummary.textContent = error.message;
                formSummary.classList.remove("d-none");
            }
        }
    });

    deleteButton.addEventListener("click", async () => {
        if (state.deletingId === null) {
            return;
        }

        try {
            const result = await request(`${deleteUrl}?id=${encodeURIComponent(state.deletingId)}`, {
                method: "POST"
            });
            modal.hide();
            await loadStudents();
            showPageMessage(result.message, "success");
        } catch (error) {
            deleteConfirmation.textContent = error.message;
        }
    });

    modalElement.addEventListener("hidden.bs.modal", () => {
        state.editingId = null;
        state.deletingId = null;
        clearValidation();
        setFormReadOnly(false);
        form.elements.namedItem("Id").readOnly = false;
    });

    loadStudents();
})();
