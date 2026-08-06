document.querySelectorAll(".copy-button").forEach((button) => {
    button.addEventListener("click", async () => {
        const text = button.getAttribute("data-copy-text") || "";

        try {
            await navigator.clipboard.writeText(text);
            button.classList.remove("btn-outline-secondary");
            button.classList.add("btn-secondary");
            button.textContent = "Copied";
        } catch {
            button.textContent = "Copy failed";
        }
    });
});
