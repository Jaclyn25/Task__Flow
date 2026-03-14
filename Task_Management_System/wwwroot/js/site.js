document.addEventListener("DOMContentLoaded", function () {
  // Simple splash fade-out after load
  const splash = document.getElementById("app-splash");
  if (splash) {
    setTimeout(() => {
      splash.classList.add("hide");
    }, 600);
  }
});

