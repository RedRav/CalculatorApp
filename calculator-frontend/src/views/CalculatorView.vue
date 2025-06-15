<template>
  <div>
    <div style="display: flex; justify-content: space-between">
      <button @click="logout">Выйти</button>
    </div>
    <h2>Калькулятор</h2>
    <form @submit.prevent="calculate">
      <input v-model.number="A" type="number" step="any" required />
      <select v-model="operation" required>
        <option value="+">+</option>
        <option value="-">-</option>
        <option value="*">*</option>
        <option value="/">/</option>
        <option value="pow">^</option>
        <option value="root">√</option>
      </select>
      <input v-model.number="B" type="number" step="any" required />
      <button type="submit">Вычислить</button>
    </form>
    <div v-if="result !== null" class="result-wrapper">
      <div class="result-success"><strong>Результат:</strong> {{ result }}</div>
    </div>
    <button @click="clearHistory">Очистить историю</button>
    <h3>История вычислений:</h3>
    <ul>
      <li v-for="(log, index) in logs" :key="index" class="log-item">
        <div class="log-timestamp">
          {{ new Date(log.timestamp).toLocaleString() }}
        </div>
        <div class="log-expression">
          {{ log.operand1 }} {{ operationSymbol(log.operation) }}
          {{ log.operand2 }}
        </div>
        <div class="log-result" :class="{ error: log.error }">
          <span v-if="log.error">{{ log.error }}</span>
          <span v-else>{{ log.result }}</span>
        </div>
      </li>
    </ul>
    <div v-if="error" style="color: red">Ошибка: {{ error }}</div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";
import { useStore } from "vuex";
import { useRouter } from "vue-router";
import { onMounted } from "vue";

export default defineComponent({
  setup() {
    const A = ref(0);
    const B = ref(0);
    const operation = ref("+");
    const result = ref<number | null>(null);
    const error = ref("");
    const store = useStore();
    const router = useRouter();
    const logs = ref<Array<any>>([]);

    onMounted(() => {
      loadLogs();
    });

    async function calculate() {
      error.value = "";
      result.value = null;
      try {
        const token = store.state.token;
        const response = await axios.post(
          "https://localhost:7100/api/calculator",
          {
            A: A.value,
            B: B.value,
            operation: operation.value,
          },
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        result.value = response.data.result;
      } catch (e: any) {
        error.value =
          e.response?.data?.message || e.message || "Ошибка запроса";
      }
      await loadLogs();
    }

    async function loadLogs() {
      try {
        const token = store.state.token;
        const response = await axios.get(
          "https://localhost:7100/api/calculator/logs",
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        logs.value = response.data;
      } catch (e) {
        console.error("Ошибка загрузки логов", e);
      }
    }
    async function clearHistory() {
      if (!confirm("Вы действительно хотите очистить историю вычислений?")) {
        return;
      }
      try {
        const token = store.state.token;
        await axios.delete("https://localhost:7100/api/calculator/logs", {
          headers: { Authorization: `Bearer ${token}` },
        });
        logs.value = [];
      } catch (e) {
        console.error("Ошибка при очистке истории", e);
      }
    }
    const operationSymbol = (op: string): string => {
      switch (op.toLowerCase()) {
        case "+":
          return "+";
        case "-":
          return "-";
        case "*":
          return "*";
        case "/":
          return "/";
        case "pow":
          return "^";
        case "root":
          return "√";
        default:
          return "?";
      }
    };
    const logout = () => {
      store.commit("setToken", null);
      router.push("/login");
    };

    return {
      A,
      B,
      operation,
      calculate,
      result,
      error,
      logout,
      logs,
      operationSymbol,
      clearHistory,
    };
  },
});
</script>
