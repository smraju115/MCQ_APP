document.getElementById("toggleSidebar").addEventListener("click", function () {
    const sidebar = document.getElementById("sidebar");
    const content = document.getElementById("content-area");

    sidebar.classList.toggle("sidebar-collapsed");
    content.classList.toggle("sidebar-collapsed");
});