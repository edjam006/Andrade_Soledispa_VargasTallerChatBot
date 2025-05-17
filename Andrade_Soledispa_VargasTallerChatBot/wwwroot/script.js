async function sendMessage(proveedor) {
    const input = document.getElementById(`${proveedor}-input`);
    const mensajes = document.getElementById(`${proveedor}-messages`);
    const pregunta = input.value;

    if (!pregunta.trim()) return;

    mensajes.innerHTML += `<div><strong>Tú:</strong> ${pregunta}</div>`;
    input.value = "";

    const response = await fetch("/api/Chat/preguntar", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ pregunta, proveedor }),
    });

    const data = await response.json();
    mensajes.innerHTML += `<div><strong>Bot:</strong> ${data.respuesta}</div>`;
    mensajes.scrollTop = mensajes.scrollHeight;
}

//Debido a que no tenemos muchas bases de javascript, aqui si fue necesaria la ayuda de una IA para la logica del enviado y recepcion de texto para el chatbot
