window.roadToTrid = {
    hideStartupLoader: () => {
        document.getElementById("startup-loader").hidden = true;
    },
    startupFailed: () => {
        document.querySelector(".startup-loader-spinner").hidden = true;
        document.getElementById("startup-loader-message").textContent = "Unable to start the application.";
        document.getElementById("startup-loader-reload").hidden = false;
    },
    copyText: async (text) => {
        await navigator.clipboard.writeText(text);
    },
    downloadFile: async (fileName, streamReference) => {
        const buffer = await streamReference.arrayBuffer();
        const url = URL.createObjectURL(new Blob([buffer], { type: "application/xml" }));
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
        URL.revokeObjectURL(url);
    }
};
