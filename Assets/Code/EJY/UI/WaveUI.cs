using System;
using System.Text;
using Code.Events;
using Core.GameEvent;
using TMPro;
using UnityEngine;

namespace Code.EJY.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI enemyCntText;

        private StringBuilder _sb;
        
        private void Awake()
        {
            _sb = new StringBuilder();
            uiChannel.AddListener<WaveTimerEvent>(HandleWaveTimer);
            uiChannel.AddListener<WaveInfoEvent>(HandleWaveInfo);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<WaveTimerEvent>(HandleWaveTimer);
            uiChannel.RemoveListener<WaveInfoEvent>(HandleWaveInfo);
        }

        private void HandleWaveInfo(WaveInfoEvent evt)
        {
            waveText.text = $"{evt.currentWave} Wave";
            enemyCntText.text = $"{evt.enemyCnt} Enemy";
        }

        private void HandleWaveTimer(WaveTimerEvent evt)
        {
            int min = (int)(evt.timer / 60f);
            int sec = (int)(evt.timer % 60f);

            _sb.Clear();
            _sb.Append(min.ToString("D2")).Append(":").Append(sec.ToString("D2"));
            timerText.text = _sb.ToString();
        }
    }
}