import httpx
import json

API_KEY = "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"
URL = "https://openrouter.ai/api/v1/chat/completions"
HEADERS = {
    "Authorization": f"Bearer {API_KEY}",
    "Content-Type": "application/json"
}

SYSTEM_PROMPT = """
without LaTeX
формулы должны быть 
основные термины с объяснением 
убрать символ \\
You are a physics assistant.
Explain physics clearly and briefly.
Show formulas when needed.
Solve step by step if it's a problem.
"""

def ask_physics(question: str) -> str:
    """
    Отправляет вопрос в модель и возвращает ответ как строку с реальными переносами и табуляцией.
    """
    messages = [
        {"role": "system", "content": SYSTEM_PROMPT},
        {"role": "user", "content": question}
    ]

    payload = {
        "model": "nvidia/nemotron-3-nano-30b-a3b:free",
        "messages": messages,
        "temperature": 0.2,
        "stream": True
    }

    answer = ""
    with httpx.stream("POST", URL, headers=HEADERS, json=payload, timeout=60) as r:
        for line in r.iter_lines():
            if line and line.startswith("data: "):
                data = line[6:]
                if data == "[DONE]":
                    break
                try:
                    chunk = json.loads(data)
                    delta = chunk["choices"][0]["delta"].get("content", "")
                    answer += delta
                except json.JSONDecodeError:
                    continue

    return answer